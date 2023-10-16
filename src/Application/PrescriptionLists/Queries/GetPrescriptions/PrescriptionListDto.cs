using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;

public class PrescriptionListDto : IMapFrom<PrescriptionList>
{
    public PrescriptionListDto()
    {
        Items = Array.Empty<PrescriptionItemDto>();
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<PrescriptionItemDto> Items { get; init; }
}
