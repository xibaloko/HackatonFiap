using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetPatientAppointments;

public class GetPatientAppointmentsMappingProfile : Profile
{
    public GetPatientAppointmentsMappingProfile()
    {
        CreateMap<Appointment, AppointmentResponse>()
            .ForPath(dest => dest.InitialDateHour, opt => opt.MapFrom(src => src.Schedule.InitialDateHour))
            .ForPath(dest => dest.FinalDateHour, opt => opt.MapFrom(src => src.Schedule.FinalDateHour))
            .ForPath(dest => dest.MedicalAppointmentPrice, opt => opt.MapFrom(src => src.Schedule.MedicalAppointmentPrice))
            .ForPath(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Schedule.Doctor.Name))
            .ForPath(dest => dest.IsCanceledByPatient, opt => opt.MapFrom(src => src.IsCanceledByPatient))
            .ForPath(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.CancellationReason));

        CreateMap<IEnumerable<Appointment>, GetPatientAppointmentsResponse>()
            .ForPath(dest => dest.Appointments, opt => opt.MapFrom(src => src));
    }
}