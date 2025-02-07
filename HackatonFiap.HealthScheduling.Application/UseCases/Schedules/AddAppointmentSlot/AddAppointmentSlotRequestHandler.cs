﻿using Azure;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddAppointmentSlot;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

public sealed class AddAppointmentSlotRequestHandler : IRequestHandler<AddAppointmentSlotRequest, Result>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public AddAppointmentSlotRequestHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(AddAppointmentSlotRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(x => x.Uuid == request.DoctorUuid, cancellationToken: cancellationToken);
        if (doctor is null)
        {
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found!"));
        }
        if (identityId != doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        var initialHour = request.InitialHour;
        var finalHour = request.InitialHour.AddMinutes(request.Duration);

        var initialDateHour = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, initialHour.Hour, initialHour.Minute, 0);
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

        var schedule = new Schedule(initialDateHour, initialDateHour.AddMinutes(request.Duration), doctor, request.Price);
        await _unitOfWork.ScheduleRepository.AddAsync(schedule, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
