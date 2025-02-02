using System.Text.Json;
using AutoFixture;
using HackatonFiap.EmailProvider.Worker;
using HackatonFiap.EmailProvider.Worker.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Xunit;

namespace HackatonFiap.Tests.Tests.Worker
{
    public class WorkerTests
    {
        private readonly Mock<ILogger<EmailProvider.Worker.Worker>> _loggerMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<ISendGridClient> _sendGridClientMock;
        private readonly Mock<IOptionsMonitor<SendGridOptions>> _sendGridOptionsMock;
        private readonly EmailProvider.Worker.Worker _worker;
        private readonly Fixture _fixture;

        public WorkerTests()
        {
            _loggerMock = new Mock<ILogger<EmailProvider.Worker.Worker>>();
            Mock<IOptions<RabbitMqSettings>> rabbitMqSettingsMock = new();
            _sendGridOptionsMock = new Mock<IOptionsMonitor<SendGridOptions>>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _sendGridClientMock = new Mock<ISendGridClient>();
            _fixture = new Fixture();

            // Configuração do RabbitMQ
            rabbitMqSettingsMock.Setup(m => m.Value).Returns(new RabbitMqSettings
            {
                HostName = "localhost",
                QueueName = "filaEmailHackaton"
            });

            // Configuração do SendGridOptions
            _sendGridOptionsMock.Setup(m => m.CurrentValue).Returns(new SendGridOptions
            {
                ApiKey = "fake-api-key"
            });

            _serviceProviderMock.Setup(x => x.GetService(typeof(ISendGridClient)))
                .Returns(_sendGridClientMock.Object);

            _worker = new EmailProvider.Worker.Worker(
                _loggerMock.Object,
                _serviceProviderMock.Object,
                rabbitMqSettingsMock.Object,
                _sendGridOptionsMock.Object
            );
        }

        [Fact]
        public async Task ProcessMessage_ShouldSendEmail_WhenValidMessageIsReceived()
        {
            // Arrange
            var consultaMessage = _fixture.Create<ConsultaMessageDto>();
            var messageJson = JsonSerializer.Serialize(consultaMessage);

            var sendGridResponse = new Response(System.Net.HttpStatusCode.Accepted, null, null);

            _sendGridClientMock
                .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Callback<SendGridMessage, CancellationToken>((msg, token) =>
                {
                    Assert.NotNull(msg);
                })
                .ReturnsAsync(sendGridResponse);

            // Act
            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { messageJson });
            await task!;

            // Assert
            _sendGridClientMock.Verify(
                client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessMessage_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var invalidMessage = "{ invalid json }";

            // Act
            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { invalidMessage });
            await task!;

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Erro ao processar a mensagem")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), 
                Times.Once);
        }
    }
}
