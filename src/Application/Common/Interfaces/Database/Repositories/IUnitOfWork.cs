using BallastLaneTestAssignment.Domain.Common;

namespace BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;

public interface IUnitOfWork
{
    IPrescriptionListRepository PrescriptionLists { get; }
    IPrescriptionItemsRepository PrescriptionItems { get; }

    Task<T> FindAsync<T>(int id) where T : BaseAuditableEntity;
    Task<int> AddAsync<T>(T entity) where T : BaseAuditableEntity;
    
    Task<int> CountAsync<T>() where T : BaseAuditableEntity;
}