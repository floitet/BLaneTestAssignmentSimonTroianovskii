using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;

namespace BallastLaneTestAssignment.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildPrescriptionItemsFile(IEnumerable<PrescriptionItemRecord> records);
}
