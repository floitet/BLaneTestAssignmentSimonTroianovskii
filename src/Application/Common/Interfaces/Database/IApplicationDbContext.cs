using BallastLaneTestAssignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BallastLaneTestAssignment.Application.Common.Interfaces.Database;

public interface IApplicationDbContext
{
    DbSet<PrescriptionList> PrescriptionLists { get; }

    DbSet<PrescriptionItem> PrescriptionItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
