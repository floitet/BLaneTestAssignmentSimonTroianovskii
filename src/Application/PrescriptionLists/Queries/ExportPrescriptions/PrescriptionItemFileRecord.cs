using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;

public class PrescriptionItemRecord : IMapFrom<PrescriptionItem>
{
    public string? Title { get; init; }

    public bool Done { get; init; }
}
