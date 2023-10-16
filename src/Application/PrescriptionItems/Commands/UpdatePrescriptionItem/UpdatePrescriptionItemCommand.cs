using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItem;

public record UpdatePrescriptionItemCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
    
    public string? UserId { get; set; }
}

public class UpdatePrescriptionItemCommandHandler : IRequestHandler<UpdatePrescriptionItemCommand>
{
    private readonly IUnitOfWork _context;

    public UpdatePrescriptionItemCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePrescriptionItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PrescriptionItems.GetByIdAsync(request.Id);
        
        if (entity == null)
        {
            throw new NotFoundException(nameof(PrescriptionItem), request.Id);
        }

        entity.Title = request.Title;
        entity.Done = request.Done;
        entity.LastModifiedBy = request.UserId;
        
        await _context.PrescriptionItems.UpdateAsync(entity);
    }
}
