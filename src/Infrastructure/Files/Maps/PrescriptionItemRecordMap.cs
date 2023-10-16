using System.Globalization;
using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;
using CsvHelper.Configuration;

namespace BallastLaneTestAssignment.Infrastructure.Files.Maps;

public class PrescriptionItemRecordMap : ClassMap<PrescriptionItemRecord>
{
    public PrescriptionItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
    }
}
