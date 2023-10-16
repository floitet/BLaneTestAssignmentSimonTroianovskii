using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.ValueObjects;
using BallastLaneTestAssignment.Infrastructure.Persistence.Models;

namespace BallastLaneTestAssignment.Infrastructure.Persistence.Mappings;

public static class DbEntriesMaps
{
    public static PrescriptionList ToPrescriptionList(this PrescriptionListFromDb prescriptionListFromDb)
    {
        return new PrescriptionList()
        {
            ListId = prescriptionListFromDb.ListId,
            Title = prescriptionListFromDb.Title,
            Colour = Colour.From(prescriptionListFromDb.ColourCode!),
            CreatedAt = prescriptionListFromDb.CreatedAt,
            LastModifiedAt = prescriptionListFromDb.LastModifiedAt,
            CreatedBy = prescriptionListFromDb.CreatedBy,
            LastModifiedBy = prescriptionListFromDb.LastModifiedBy,
            Items = prescriptionListFromDb.Items
        };
    }
    
}