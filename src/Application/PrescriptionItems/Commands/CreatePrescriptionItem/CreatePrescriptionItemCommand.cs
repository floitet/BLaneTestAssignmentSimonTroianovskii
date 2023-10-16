using System.Text.Json.Serialization;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.Events;
using MediatR;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;

public record CreatePrescriptionItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
    
    [JsonIgnore]
    public string? CreatedBy { get; set; }

    public bool Done { get; set; }
}

public class CreatePrescriptionItemCommandHandler : IRequestHandler<CreatePrescriptionItemCommand, int>
{
    private readonly IUnitOfWork _context;

    public CreatePrescriptionItemCommandHandler(IUnitOfWork context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePrescriptionItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new PrescriptionItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = request.Done, 
            CreatedBy = request.CreatedBy
        };

        entity.AddDomainEvent(new PrescriptionItemCreatedEvent(entity));

        var existing = await _context.PrescriptionItems.FindByTitleAsync(entity.Title);

        if (existing != null)
        {
            return existing.Id;
        }
        
        var insertedId = await _context.PrescriptionItems.AddAsync(entity);
        
        return insertedId;
    }
}
