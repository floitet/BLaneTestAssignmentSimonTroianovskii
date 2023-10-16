using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<PrescriptionList>
{
    public int Id { get; init; }

    public string? Title { get; init; }
}
