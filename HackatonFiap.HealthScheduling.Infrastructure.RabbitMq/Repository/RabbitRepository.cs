using System.Text;
using System.Text.Json;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Entities;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using RabbitMQ.Client;

namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        private readonly RabbitMqSettings _rabbitMqQueue;

        public RabbitRepository(RabbitMqSettings rabbitMqQueue)
        {
            _rabbitMqQueue = rabbitMqQueue;
        }

        public async Task EnviarMensagem(string nomeMedico,
            string emailMedico,
            string nomePaciente,
            string dataConsulta,
            string horaConsulta)
        {
            try
            {
                var dto = new ConsultaMessageDto(nomeMedico, emailMedico, nomePaciente, dataConsulta, horaConsulta);
                var factory = new ConnectionFactory { HostName = _rabbitMqQueue.HostName };
                var connection = await factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();
                
                await channel.QueueDeclareAsync(_rabbitMqQueue.QueueName, true, false, false,  null);
                
                var mensagemJson = JsonSerializer.Serialize(dto);
                var body = Encoding.UTF8.GetBytes(mensagemJson);
                
                await channel.BasicPublishAsync("", _rabbitMqQueue.QueueName, body);
                    
                
                Console.WriteLine($"[Producer] Mensagem enviada: {mensagemJson}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}
