//using System.Text.Json;
//using AutoFixture;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Moq;

//namespace HackatonFiap.Tests.Tests.Worker
//{
//    public class WorkerTests
//    {
//        private readonly Mock<ILogger<EmailProvider.Worker.Worker>> _loggerMock;
//        private readonly Mock<ISendGridClient> _sendGridClientMock;
//        private readonly Mock<IOptionsMonitor<SendGridOptions>> _sendGridOptionsMock;
//        private readonly EmailProvider.Worker.Worker _worker;
//        private readonly Fixture _fixture;

//        public WorkerTests()
//        {
//            _loggerMock = new Mock<ILogger<EmailProvider.Worker.Worker>>();
//            Mock<IOptions<RabbitMqSettings>> rabbitMqSettingsMock = new();
//            _sendGridOptionsMock = new Mock<IOptionsMonitor<SendGridOptions>>();
//            _sendGridClientMock = new Mock<ISendGridClient>();
//            _fixture = new Fixture();

//            // Configuração do RabbitMQ
//            rabbitMqSettingsMock.Setup(m => m.Value).Returns(new RabbitMqSettings
//            {
//                HostName = "localhost",
//                QueueName = "filaEmailHackaton"
//            });

//            // Configuração do SendGridOptions
//            _sendGridOptionsMock.Setup(m => m.CurrentValue).Returns(new SendGridOptions
//            {
//                ApiKey = "fake-api-key"
//            });

//            _worker = new EmailProvider.Worker.Worker(
//                _loggerMock.Object,
//                rabbitMqSettingsMock.Object,
//                _sendGridOptionsMock.Object,
//                _sendGridClientMock.Object
//            );
//        }

//        [Fact]
//        public async Task ProcessMessage_ShouldSendEmail_WhenValidMessageIsReceived()
//        {
//            // Arrange
//            var consultaMessage = _fixture.Create<ConsultaMessageDto>();
//            var messageJson = JsonSerializer.Serialize(consultaMessage);

//            var sendGridResponse = new Response(System.Net.HttpStatusCode.Accepted, null, null);

//            _sendGridClientMock
//                .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(sendGridResponse);

//            // Act
//            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
//                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { messageJson });
//            await task!;

//            // Assert
//            _sendGridClientMock.Verify(
//                client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
//                Times.Once);
//        }

//        [Fact]
//        public async Task ProcessMessage_ShouldLogError_WhenInvalidJsonMessageIsReceived()
//        {
//            // Arrange
//            var invalidMessage = "{ invalid json }";

//            // Act
//            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
//                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { invalidMessage });
//            await task!;

//            // Assert
//            _loggerMock.Verify(
//                x => x.Log(
//                    LogLevel.Error,
//                    It.IsAny<EventId>(),
//                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Erro ao processar a mensagem")),
//                    It.IsAny<Exception>(),
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), 
//                Times.Once);
//        }

//        [Fact]
//        public async Task ProcessMessage_ShouldLogError_WhenSendGridApiKeyIsMissing()
//        {
//            // Arrange
//            _sendGridOptionsMock.Setup(m => m.CurrentValue).Returns(new SendGridOptions { ApiKey = string.Empty });

//            var consultaMessage = _fixture.Create<ConsultaMessageDto>();
//            var messageJson = JsonSerializer.Serialize(consultaMessage);

//            // Act
//            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
//                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { messageJson });
//            await task!;

//            // Assert
//            _loggerMock.Verify(
//                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>(
//                    (o, t) => o.ToString()!.Contains("SendGrid API Key não configurada.")), null, 
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), 
//                Times.Once);
//        }

//        [Fact]
//        public async Task ProcessMessage_ShouldLogError_WhenSendGridClientIsNull()
//        {
//            // Arrange
//            var consultaMessage = _fixture.Create<ConsultaMessageDto>();
//            var messageJson = JsonSerializer.Serialize(consultaMessage);

//            _sendGridClientMock.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
//                .ThrowsAsync(new Exception("Erro ao enviar e-mail"));

//            // Act
//            var processMessageMethod = typeof(EmailProvider.Worker.Worker)
//                .GetMethod("ProcessMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//            var task = (Task?)processMessageMethod?.Invoke(_worker, new object[] { messageJson });
//            await task!;

//            // Assert
//            _loggerMock.Verify(
//                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>(
//                    (o, t) => o.ToString()!.Contains("Erro ao processar a mensagem.")), 
//                    It.IsAny<Exception>(), 
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), 
//                Times.Once);
//        }
//    }
//}
