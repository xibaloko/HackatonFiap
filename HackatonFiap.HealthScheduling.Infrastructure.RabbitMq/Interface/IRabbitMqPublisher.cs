
namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;

public interface IRabbitMqPublisher
{
    Task EnviarMensagem(
        string nomeMedico,
        string emailMedico,
        string nomePaciente,
        string dataConsulta,
        string horaConsulta);
}