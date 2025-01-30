using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed record GetAllDoctorsRequest : IRequest<List<GetAllDoctorsResponse>>
{

}
