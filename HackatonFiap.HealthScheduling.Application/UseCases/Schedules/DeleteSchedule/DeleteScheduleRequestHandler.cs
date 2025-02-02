using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed class DeleteScheduleRequestHandler : IRequestHandler<DeleteScheduleRequest, Result>
{
    private readonly IRepositories _repositories;

    public DeleteScheduleRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(DeleteScheduleRequest request, CancellationToken cancellationToken)
    {
        var schedule = await _repositories.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.Uuid && x.IsDeleted==false
                                                                                    , cancellationToken: cancellationToken);
        if (schedule == null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not found!"));

        schedule.AsSoftDeletable();
        _repositories.ScheduleRepository.Update(schedule);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
