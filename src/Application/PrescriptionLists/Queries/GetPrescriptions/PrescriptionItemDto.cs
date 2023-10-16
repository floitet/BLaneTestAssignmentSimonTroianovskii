using AutoMapper;
using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;

public class PrescriptionItemDto : IMapFrom<PrescriptionItem>
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public int Priority { get; init; }

    public string? Note { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PrescriptionItem, PrescriptionItemDto>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
    }
}
