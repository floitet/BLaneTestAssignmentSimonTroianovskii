using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Commands.DeletePrescriptionList;

public record DeletePrescriptionListCommand(int Id) : IRequest;

public class DeletePrescriptionListCommandHandler : IRequestHandler<DeletePrescriptionListCommand>
{
    private readonly IUnitOfWork _context;

    public DeletePrescriptionListCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task Handle(DeletePrescriptionListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PrescriptionLists.GetByIdAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(PrescriptionList), request.Id);
        }

        await _context.PrescriptionLists.DeleteAsync(entity.ListId);
    }
}
