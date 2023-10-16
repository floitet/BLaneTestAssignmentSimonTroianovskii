namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;

public class PrescriptionsVm
{
    public IReadOnlyCollection<PriorityLevelDto> PriorityLevels { get; init; } = Array.Empty<PriorityLevelDto>();

    public IReadOnlyCollection<PrescriptionListDto> Lists { get; init; } = Array.Empty<PrescriptionListDto>();
}
