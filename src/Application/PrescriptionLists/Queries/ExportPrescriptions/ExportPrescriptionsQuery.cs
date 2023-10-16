using AutoMapper;
using AutoMapper.QueryableExtensions;
using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;

public record ExportPrescriptionsQuery : IRequest<ExportPrescriptionsVm>
{
    public int ListId { get; init; }
}

public class ExportPrescriptionsQueryHandler : IRequestHandler<ExportPrescriptionsQuery, ExportPrescriptionsVm>
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;
    private readonly ICsvFileBuilder _fileBuilder;

    public ExportPrescriptionsQueryHandler(IUnitOfWork context, IMapper mapper, ICsvFileBuilder fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    public async Task<ExportPrescriptionsVm> Handle(ExportPrescriptionsQuery request, CancellationToken cancellationToken)
    {

        var records = (await _context.PrescriptionItems
            .GetByPrescriptionListIdAsync(request.ListId)).Select(pi => _mapper.Map<PrescriptionItemRecord>(pi))
            .ToList();
        
        var vm = new ExportPrescriptionsVm(
            "PrescriptionItems.csv",
            "text/csv",
            _fileBuilder.BuildPrescriptionItemsFile(records));

        return vm;
    }
}
