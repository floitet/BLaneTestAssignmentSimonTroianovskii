using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Queries.GetPrescriptionItemsWithPagination;

public class PrescriptionItemBriefDto : IMapFrom<PrescriptionItem>
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}
