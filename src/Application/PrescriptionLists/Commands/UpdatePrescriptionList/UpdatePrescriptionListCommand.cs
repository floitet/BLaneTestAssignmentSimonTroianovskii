using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Commands.UpdatePrescriptionList;

public record UpdatePrescriptionListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
    
    public string? UserId { get; set; }
}

public class UpdatePrescriptionListCommandHandler : IRequestHandler<UpdatePrescriptionListCommand>
{
    private readonly IUnitOfWork _context;

    public UpdatePrescriptionListCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePrescriptionListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PrescriptionLists.GetByIdAsync(request.Id);
        
        if (entity == null)
        {
            throw new NotFoundException(nameof(PrescriptionList), request.Id);
        }

        entity.Title = request.Title;
        entity.LastModifiedBy = request.UserId;
        
        await _context.PrescriptionLists.UpdateAsync(entity);


    }
}
