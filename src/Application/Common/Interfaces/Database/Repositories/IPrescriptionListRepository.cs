using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;

public interface IPrescriptionListRepository : IGenericRepository<PrescriptionList>
{
    public Task<int> GetPrescriptionListCountByTitle(string title);
}