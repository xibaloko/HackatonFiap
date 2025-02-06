using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.UpdateSchedule;

public sealed class UpdateScheduleRequestHandler : IRequestHandler<UpdateScheduleRequest, Result>
{
    private readonly IRepositories _repositories;

    public UpdateScheduleRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(UpdateScheduleRequest request, CancellationToken cancellationToken)
    {
        Schedule? schedule = await _repositories.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.Uuid, "Doctor", cancellationToken: cancellationToken);
        if (schedule is null)
        {
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not found!"));
        }

        var initialHour = request.InitialHour;
        var finalHour = request.FinalHour;
        var initialDateHour = new DateTime(schedule.InitialDateHour.Year, schedule.InitialDateHour.Month, schedule.InitialDateHour.Day, initialHour.Hour, initialHour.Minute, 0);
        var finalDateHour = new DateTime(schedule.FinalDateHour.Year, schedule.FinalDateHour.Month, schedule.FinalDateHour.Day, finalHour.Hour, finalHour.Minute, 0);

        var duplicateds = await _repositories.ScheduleRepository.GetAllAsync(x =>
                                                                x.InitialDateHour < finalDateHour
                                                                && x.FinalDateHour > initialDateHour
                                                                && x.Doctor.Uuid == schedule.Doctor.Uuid
                                                                && x.IsDeleted == false
                                                                && x.Uuid != request.Uuid
                                                                , cancellationToken: cancellationToken);
        if (duplicateds.Any())
        {
            return Result.Fail(ErrorHandler.HandleConflict($"Exists Schedules conflicted between {initialDateHour} and {finalDateHour}!"));
        }
        schedule.RescheduleAppointment(initialDateHour, finalDateHour, request.Price);
        _repositories.ScheduleRepository.Update(schedule);
        await _repositories.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
