using System.Globalization;
using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;
using BallastLaneTestAssignment.Infrastructure.Files.Maps;
using CsvHelper;

namespace BallastLaneTestAssignment.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildPrescriptionItemsFile(IEnumerable<PrescriptionItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<PrescriptionItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
