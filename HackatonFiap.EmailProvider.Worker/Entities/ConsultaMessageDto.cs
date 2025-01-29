namespace HackatonFiap.EmailProvider.Worker;

public record ConsultaMessageDto(
    string NomeMedico,
    string EmailMedico,
    string NomePaciente,
    string DataConsulta,
    string HoraConsulta
);