using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Common;
using BallastLaneTestAssignment.Domain.Entities;

namespace BallastLaneTestAssignment.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IPrescriptionListRepository PrescriptionLists { get; }
    public IPrescriptionItemsRepository PrescriptionItems { get; }
    

    public UnitOfWork(IPrescriptionListRepository prescriptionLists, 
        IPrescriptionItemsRepository prescriptionItems)
    {
        PrescriptionLists = prescriptionLists;
        PrescriptionItems = prescriptionItems;
    }
    
    public async Task<T> FindAsync<T>(int id) where T : BaseAuditableEntity
    {
        return typeof(T).Name switch
        {
            nameof(PrescriptionList) => ((await PrescriptionLists.GetByIdAsync(id)) as T)!,
            nameof(PrescriptionItem) => ((await PrescriptionItems.GetByIdAsync(id)) as T)!,
            _ => throw new InvalidOperationException($"Type {typeof(T).Name} is not supported.")
        };
    }

    public async Task<int> AddAsync<T>(T entity) where T : BaseAuditableEntity
    {
        return typeof(T).Name switch
        {
            nameof(PrescriptionList) => await PrescriptionLists.AddAsync((entity as PrescriptionList)!),
            nameof(PrescriptionItem) => await PrescriptionItems.AddAsync((entity as PrescriptionItem)!),
            _ => throw new InvalidOperationException($"Type {typeof(T).Name} is not supported.")
        };
    }

    public async Task<int> CountAsync<T>() where T : BaseAuditableEntity
    {
        return typeof(T).Name switch
        {
            nameof(PrescriptionList) => (await PrescriptionLists.GetAllAsync()).Count,
            nameof(PrescriptionItem) => (await PrescriptionItems.GetAllAsync()).Count,
            _ => throw new InvalidOperationException($"Type {typeof(T).Name} is not supported.")
        };
    }
}