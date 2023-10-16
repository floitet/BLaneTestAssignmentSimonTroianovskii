namespace BallastLaneTestAssignment.Domain.Events;

public class PrescriptionItemCreatedEvent : BaseEvent
{
    public PrescriptionItemCreatedEvent(PrescriptionItem item)
    {
        Item = item;
    }

    public PrescriptionItem Item { get; }
}
