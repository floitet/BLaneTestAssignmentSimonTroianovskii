using AutoMapper;
using AutoMapper.QueryableExtensions;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Application.Common.Models;
using BallastLaneTestAssignment.Domain.Entities;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Queries.GetPrescriptionItemsWithPagination;

public record GetPrescriptionItemsWithPaginationQuery : IRequest<PaginatedList<PrescriptionItemBriefDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPrescriptionItemsWithPaginationQueryHandler : IRequestHandler<GetPrescriptionItemsWithPaginationQuery, PaginatedList<PrescriptionItemBriefDto>>
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;

    public GetPrescriptionItemsWithPaginationQueryHandler(IUnitOfWork context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PrescriptionItemBriefDto>> Handle(GetPrescriptionItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var items = await (await _context.PrescriptionItems.GetByPrescriptionListIdAsync(request.ListId))
            .Select(pi => _mapper.Map<PrescriptionItemBriefDto>(pi))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return items;




    }
}
