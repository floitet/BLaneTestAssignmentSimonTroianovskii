using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.Events;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.DeletePrescriptionItem;

public record DeletePrescriptionItemCommand(int Id) : IRequest;

public class DeletePrescriptionItemCommandHandler : IRequestHandler<DeletePrescriptionItemCommand>
{
    private readonly IUnitOfWork _context;

    public DeletePrescriptionItemCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(DeletePrescriptionItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PrescriptionItems.GetByIdAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(PrescriptionItem), request.Id);
        }

        await _context.PrescriptionItems.DeleteAsync(entity.Id);

        entity.AddDomainEvent(new PrescriptionItemDeletedEvent(entity));
        
    }

}
