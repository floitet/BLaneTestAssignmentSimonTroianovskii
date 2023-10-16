namespace BallastLaneTestAssignment.Domain.Events;

public class PrescriptionItemCompletedEvent : BaseEvent
{
    public PrescriptionItemCompletedEvent(PrescriptionItem item)
    {
        Item = item;
    }

    public PrescriptionItem Item { get; }
}
