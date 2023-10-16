using AutoMapper;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Application.Common.Security;
using BallastLaneTestAssignment.Domain.Enums;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;

[Authorize]
public record GetPrescriptionsQuery : IRequest<PrescriptionsVm>;

public class GetPrescriptionsQueryHandler : IRequestHandler<GetPrescriptionsQuery, PrescriptionsVm>
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;

    public GetPrescriptionsQueryHandler(IUnitOfWork context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PrescriptionsVm> Handle(GetPrescriptionsQuery request, CancellationToken cancellationToken)
    {
        
        var prescriptionLists = await _context.PrescriptionLists.GetAllAsync();
        var prescriptionListsMapped = prescriptionLists.Select(p => 
            _mapper.Map<PrescriptionListDto>(p)).ToList();
        
        return new PrescriptionsVm
        {
            PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new PriorityLevelDto { Value = (int)p, Name = p.ToString() })
                .ToList(),

            Lists = prescriptionListsMapped
        };
    }
}
