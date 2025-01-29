
namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;

public interface IRabbitRepository
{
    Task EnviarMensagem(
        string nomeMedico,
        string emailMedico,
        string nomePaciente,
        string dataConsulta,
        string horaConsulta);
}