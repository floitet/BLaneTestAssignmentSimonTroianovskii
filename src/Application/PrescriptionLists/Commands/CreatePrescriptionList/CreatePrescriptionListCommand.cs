using System.Text.Json.Serialization;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;

public record CreatePrescriptionListCommand : IRequest<int>
{
    public string? Title { get; init; }
    [JsonIgnore]
    public string? UserId { get; set; }
}

public class CreatePrescriptionListCommandHandler : IRequestHandler<CreatePrescriptionListCommand, int>
{
    private readonly IUnitOfWork _context;

    public CreatePrescriptionListCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePrescriptionListCommand request, CancellationToken cancellationToken)
    {
        var entity = new PrescriptionList();

        entity.Title = request.Title;
        entity.CreatedBy = request.UserId;

        var insertedId = await _context.PrescriptionLists.AddAsync(entity);
        
        return insertedId;
    }
}
