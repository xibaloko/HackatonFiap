using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Agendas;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed class AddScheduleRequestHandler : IRequestHandler<AddScheduleRequest, Result>
{
    private readonly IRepositories _repositories;

    public AddScheduleRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;

    }

    public async Task<Result> Handle(AddScheduleRequest request, CancellationToken cancellationToken)
    {
        var horaini = request.InitialHour;
        var horaFim = request.FinalHour;
        var periodo = (horaFim - horaini).TotalMinutes;
        var quantidadeConsultas = (int)(periodo / request.Duration);

        Doctor? doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(x => x.Uuid == request.DoctorUuid, cancellationToken: cancellationToken);

        var scheduleEntitys = new List<Schedule>();

        var dataAux = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, horaini.Hour, horaini.Minute, 0);
        for (int i = 1; i <= quantidadeConsultas; i++)
        {
            scheduleEntitys.Add(new Schedule(dataAux, request.Duration, doctor));
            dataAux = dataAux.AddMinutes(request.Duration);
        }

        await _repositories.ScheduleRepository.AddBulkAssync(scheduleEntitys);
        await _repositories.SaveAsync(cancellationToken);
     
        return Result.Ok();
    }
}
