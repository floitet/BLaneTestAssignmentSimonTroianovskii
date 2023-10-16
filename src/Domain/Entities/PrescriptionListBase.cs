namespace BallastLaneTestAssignment.Domain.Entities;

public class PrescriptionListBase : BaseAuditableEntity
{
    public string? Title { get; set; }
    public IList<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}