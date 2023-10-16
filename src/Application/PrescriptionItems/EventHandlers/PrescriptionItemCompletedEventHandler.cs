using BallastLaneTestAssignment.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.EventHandlers;

public class PrescriptionItemCompletedEventHandler : INotificationHandler<PrescriptionItemCompletedEvent>
{
    private readonly ILogger<PrescriptionItemCompletedEventHandler> _logger;

    public PrescriptionItemCompletedEventHandler(ILogger<PrescriptionItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PrescriptionItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BallastLaneTestAssignment Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
