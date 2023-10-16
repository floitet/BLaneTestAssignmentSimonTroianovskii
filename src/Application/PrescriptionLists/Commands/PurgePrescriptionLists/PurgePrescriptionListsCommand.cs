using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Application.Common.Security;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Commands.PurgePrescriptionLists;

[Authorize(Roles = "Administrator")]
[Authorize(Policy = "CanPurge")]
public record PurgePrescriptionListsCommand : IRequest;

public class PurgePrescriptionListsCommandHandler : IRequestHandler<PurgePrescriptionListsCommand>
{
    private readonly IUnitOfWork _context;

    public PurgePrescriptionListsCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(PurgePrescriptionListsCommand request, CancellationToken cancellationToken)
    {
       await _context.PrescriptionLists.DeleteAllAsync();
    }
}
