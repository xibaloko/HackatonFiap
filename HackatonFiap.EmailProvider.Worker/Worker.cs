using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.Json;
using RabbitMQ.Client.Events;
using System.Text;
using HackatonFiap.EmailProvider.Worker.Configurations;

namespace HackatonFiap.EmailProvider.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqSettings _rabbitMqQueue;
        private readonly IOptionsMonitor<SendGridOptions> _sendGridOptions;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<RabbitMqSettings> rabbitMqOptions, IOptionsMonitor<SendGridOptions> sendGridOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _rabbitMqQueue = rabbitMqOptions.Value;
            _sendGridOptions = sendGridOptions;
        }


        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _rabbitMqQueue.HostName };
            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(queue: _rabbitMqQueue.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Mensagem recebida: {message}", message);
                }

                ProcessMessage(message).GetAwaiter().GetResult();
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: _rabbitMqQueue.QueueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken);

            return Task.CompletedTask;
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                var consultaMessage = JsonSerializer.Deserialize<ConsultaMessageDto>(message);
                if (consultaMessage == null)
                {
                    _logger.LogError("Mensagem Vazia");
                    return;
                }

                var sendGridOptions = _sendGridOptions.CurrentValue;
                if (string.IsNullOrEmpty(sendGridOptions?.ApiKey))
                {
                    _logger.LogError("SendGrid API Key não configurada.");
                    return;
                }


                var client = _serviceProvider.GetService<ISendGridClient>();
                if (client == null)
                {
                    return;
                }

                var from = new EmailAddress("heiterpm@gmail.com", "Robo 52");
                var to = new EmailAddress(consultaMessage.EmailMedico, consultaMessage.EmailMedico);

                const string subject = "Health&Med - Nova consulta agendada";
                var plainTextContent = $"Olá, Dr. {consultaMessage.NomeMedico}!\nVocê tem uma nova consulta marcada!\nPaciente: {consultaMessage.NomePaciente}.\nData e horário: {consultaMessage.DataConsulta} às {consultaMessage.HoraConsulta}.";
                var htmlContent = $"<p>Olá, Dr. {consultaMessage.NomeMedico}!</p><p>Você tem uma nova consulta marcada!</p><p><b>Paciente:</b> {consultaMessage.NomePaciente}</p><p><b>Data e horário:</b> {consultaMessage.DataConsulta} às {consultaMessage.HoraConsulta}.</p>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                await client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem.");
            }
        }

    }

}
