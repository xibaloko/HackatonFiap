using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.UpdateSchedule;

public sealed class UpdateScheduleRequestHandler : IRequestHandler<UpdateScheduleRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateScheduleRequestHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(UpdateScheduleRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Schedule? schedule = await _unitOfWork.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.Uuid, "Doctor", cancellationToken: cancellationToken);
        if (schedule is null)
        {
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not found!"));
        }

        if (identityId != schedule.Doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        var initialHour = request.InitialHour;
        var finalHour = request.FinalHour;
        var initialDateHour = new DateTime(schedule.InitialDateHour.Year, schedule.InitialDateHour.Month, schedule.InitialDateHour.Day, initialHour.Hour, initialHour.Minute, 0);
        var finalDateHour = new DateTime(schedule.FinalDateHour.Year, schedule.FinalDateHour.Month, schedule.FinalDateHour.Day, finalHour.Hour, finalHour.Minute, 0);

        var duplicateds = await _unitOfWork.ScheduleRepository.GetAllAsync(x =>
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
        _unitOfWork.ScheduleRepository.Update(schedule);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
