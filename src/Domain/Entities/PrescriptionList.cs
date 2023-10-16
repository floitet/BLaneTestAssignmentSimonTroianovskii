namespace BallastLaneTestAssignment.Domain.Entities;

public class PrescriptionList : PrescriptionListBase
{
    public int ListId { get; set; }
    public Colour Colour { get; set; } = Colour.White;
    
}
