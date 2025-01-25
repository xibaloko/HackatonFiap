namespace HackatonFiap.EmailProvider.Worker;

public record ConsultaMessage(
    string NomeMedico,
    string EmailMedico,
    string NomePaciente,
    string EmailPaciente,
    string DataConsulta,
    string HoraConsulta
);