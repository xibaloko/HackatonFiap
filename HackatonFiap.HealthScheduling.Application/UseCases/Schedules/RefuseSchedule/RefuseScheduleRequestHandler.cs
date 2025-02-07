using FluentResults;
using MediatR;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.RefuseSchedule;

public class RefuseScheduleRequestHandler : IRequestHandler<RefuseScheduleRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefuseScheduleRequestHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(RefuseScheduleRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Schedule? schedule = await _unitOfWork.ScheduleRepository.FirstOrDefaultAsync(schedule =>
            schedule.Uuid == request.ScheduleUuid && schedule.IsDeleted == false, includeProperties: "Doctor,Appointments", cancellationToken: cancellationToken);

        if (schedule is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not found."));

        if (identityId != schedule.Doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        schedule.AsSoftDeletable();
        _unitOfWork.ScheduleRepository.Update(schedule);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
