using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;

public interface IPrescriptionItemsRepository : IGenericRepository<PrescriptionItem>
{
    Task<int> UpdateDetailsAsync(PrescriptionItem entity);
    
    Task<IReadOnlyList<PrescriptionItem>> GetByPrescriptionListIdAsync(int prescriptionListId);
    
    Task<PrescriptionItem?> FindByTitleAsync(string title);
}