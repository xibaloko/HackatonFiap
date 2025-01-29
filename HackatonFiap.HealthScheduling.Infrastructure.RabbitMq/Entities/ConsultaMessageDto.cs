namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Entities;

public record ConsultaMessageDto(
    string NomeMedico,
    string EmailMedico,
    string NomePaciente,
    string DataConsulta,
    string HoraConsulta
);