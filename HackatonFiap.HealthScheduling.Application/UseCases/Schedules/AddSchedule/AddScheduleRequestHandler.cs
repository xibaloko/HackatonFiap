using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
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
        Doctor? doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(x => x.Uuid == request.DoctorUuid, cancellationToken: cancellationToken);
        if (doctor is null)
        {
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found!"));
        }

        var initHour = request.InitialHour;
        var finalHour = request.FinalHour;
        var period = (finalHour - initHour).TotalMinutes;
        
        var quantitySchedules = (int)(period / request.Duration);
        
        var dtini = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, initHour.Hour, initHour.Minute, 0);
        var dtFim = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, finalHour.Hour, finalHour.Minute, 0);

        var duplicateds = await _repositories.ScheduleRepository.GetAllAsync(x =>
                                                                x.DateHour >= dtini && x.DateHour <= dtFim
                                                                && x.Doctor.Uuid == request.DoctorUuid
                                                                , cancellationToken: cancellationToken);
        if (duplicateds.Any())
        {
            return Result.Fail(ErrorHandler.HandleConflict($"Exists Schedules conflicted between {dtini} and {dtFim}!"));
        }

        var scheduleEntitys = new List<Schedule>();
        for (int i = 1; i <= quantitySchedules; i++)
        {
            scheduleEntitys.Add(new Schedule(dtini, request.Duration, doctor));
            dtini = dtini.AddMinutes(request.Duration);
        }

        await _repositories.ScheduleRepository.AddBulkAsync(scheduleEntitys);
        await _repositories.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
