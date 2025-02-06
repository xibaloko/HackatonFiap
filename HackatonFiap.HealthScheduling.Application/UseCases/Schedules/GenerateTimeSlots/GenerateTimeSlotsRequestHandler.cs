using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

public sealed class GenerateTimeSlotsRequestHandler : IRequestHandler<GenerateTimeSlotsRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public GenerateTimeSlotsRequestHandler(IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(GenerateTimeSlotsRequest request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(x => x.Uuid == request.DoctorUuid, cancellationToken: cancellationToken);
        if (doctor is null)
        {
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found!"));
        }

        var initHour = request.InitialHour;
        var finalHour = request.FinalHour;
        var period = (finalHour - initHour).TotalMinutes;

        var quantitySchedules = (int)(period / request.Duration);

        var initialDateHour = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, initHour.Hour, initHour.Minute, 0);
        var finalDateHour = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, finalHour.Hour, finalHour.Minute, 0);

        var duplicateds = await _unitOfWork.ScheduleRepository.GetAllAsync(x =>
                                                                x.InitialDateHour < finalDateHour
                                                                && x.FinalDateHour > initialDateHour
                                                                && x.Doctor.Uuid == request.DoctorUuid
                                                                && x.IsDeleted == false
                                                                , cancellationToken: cancellationToken);
        if (duplicateds.Any())
        {
            return Result.Fail(ErrorHandler.HandleConflict($"Exists Schedules conflicted between {initialDateHour} and {finalDateHour}!"));
        }

        var scheduleEntitys = new List<Schedule>();
        for (int i = 1; i <= quantitySchedules; i++)
        {
            scheduleEntitys.Add(new Schedule(initialDateHour, initialDateHour.AddMinutes(request.Duration), doctor, request.Price));
            initialDateHour = initialDateHour.AddMinutes(request.Duration);
        }

        await _unitOfWork.ScheduleRepository.AddBulkAsync(scheduleEntitys, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
