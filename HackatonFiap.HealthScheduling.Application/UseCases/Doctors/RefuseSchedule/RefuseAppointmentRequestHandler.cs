using FluentResults;
using MediatR;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseSchedule;

public class RefuseAppointmentRequestHandler : IRequestHandler<RefuseScheduleRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RefuseAppointmentRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RefuseScheduleRequest request, CancellationToken cancellationToken)
    {
        Schedule? schedule = await _unitOfWork.ScheduleRepository.FirstOrDefaultAsync(schedule =>
            schedule.Uuid == request.ScheduleUuid, includeProperties: "Appointment", cancellationToken: cancellationToken);

        if (schedule is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not found."));

        schedule.AsSoftDeletable();
        schedule.Appointment?.AsSoftDeletable();

        _unitOfWork.ScheduleRepository.Update(schedule);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
