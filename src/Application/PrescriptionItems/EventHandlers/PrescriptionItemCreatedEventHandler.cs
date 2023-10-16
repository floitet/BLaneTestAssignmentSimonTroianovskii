using BallastLaneTestAssignment.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.EventHandlers;

public class PrescriptionItemCreatedEventHandler : INotificationHandler<PrescriptionItemCreatedEvent>
{
    private readonly ILogger<PrescriptionItemCreatedEventHandler> _logger;

    public PrescriptionItemCreatedEventHandler(ILogger<PrescriptionItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PrescriptionItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BallastLaneTestAssignment Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
