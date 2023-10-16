namespace BallastLaneTestAssignment.Domain.Events;

public class PrescriptionItemDeletedEvent : BaseEvent
{
    public PrescriptionItemDeletedEvent(PrescriptionItem item)
    {
        Item = item;
    }

    public PrescriptionItem Item { get; }
}
