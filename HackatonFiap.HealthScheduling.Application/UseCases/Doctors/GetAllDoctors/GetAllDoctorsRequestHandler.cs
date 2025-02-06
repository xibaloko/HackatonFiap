using System.Linq.Expressions;
using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public class GetAllDoctorsRequestHandler : IRequestHandler<GetAllDoctorsRequest, Result<GetAllDoctorsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDoctorsRequestHandler(IUnitOfWork repositories, IMapper mapper)
    {
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllDoctorsResponse>> Handle(GetAllDoctorsRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<Doctor, bool>> baseFilter = d => !d.IsDeleted;
        Expression<Func<Doctor, bool>>? specialtyFilter = null;

        if (!string.IsNullOrEmpty(request.Specialty))
        {
            string normalizedSpecialty = request.Specialty.ToLower();

            specialtyFilter = d => d.MedicalSpecialty != null && 
                                   d.MedicalSpecialty.Description.ToLower() == normalizedSpecialty;
        }

        var finalFilter = specialtyFilter != null 
            ? CombineExpressions(baseFilter, specialtyFilter) 
            : baseFilter;

        var doctors = await _unitOfWork.DoctorRepository.GetAllAsync(
            filter: finalFilter,
            includeProperties: "MedicalSpecialty",
            isTracking: false,
            cancellationToken: cancellationToken
        );

        var response = _mapper.Map<GetAllDoctorsResponse>(doctors);
        return Result.Ok(response);
    }

    
    private static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var param = Expression.Parameter(typeof(T), "x");

        var combined = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param)
            ), param);

        return combined;
    }


}