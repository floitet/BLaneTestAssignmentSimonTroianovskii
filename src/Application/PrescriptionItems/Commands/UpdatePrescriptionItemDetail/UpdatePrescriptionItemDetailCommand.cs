using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.Enums;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItemDetail;

public record UpdatePrescriptionItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

public class UpdatePrescriptionItemDetailCommandHandler : IRequestHandler<UpdatePrescriptionItemDetailCommand>
{
    private readonly IUnitOfWork _context;

    public UpdatePrescriptionItemDetailCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePrescriptionItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PrescriptionItems.GetByIdAsync(request.Id);
        
        if (entity == null)
        {
            throw new NotFoundException(nameof(PrescriptionItem), request.Id);
        }

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await _context.PrescriptionItems.UpdateDetailsAsync(entity);
    }
}
