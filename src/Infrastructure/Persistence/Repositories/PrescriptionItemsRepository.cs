using System.Data.Common;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using Dapper;

namespace BallastLaneTestAssignment.Infrastructure.Persistence.Repositories;

public class PrescriptionItemsRepository : IPrescriptionItemsRepository
{
    private readonly DbConnection _connection;

    public PrescriptionItemsRepository(DbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<IReadOnlyList<PrescriptionItem>> GetAllAsync()
    {
        const string query = "SELECT * FROM [PrescriptionItem]";
        var results = (await _connection.QueryAsync<PrescriptionItem>(query)).ToList();
        return results.AsList();
    }

    public async Task<PrescriptionItem?> GetByIdAsync(int id)
    {
        const string query = "SELECT  [Id], [ListId], " +
                             "[Title], [Note], [Priority], " +
                             "[Reminder], [Done], [CreatedAt], " +
                             "[LastModifiedAt], [CreatedBy], [LastModifiedBy] " +
                             "FROM [PrescriptionItem] WHERE Id = @Id";
        var result = await _connection.QueryFirstOrDefaultAsync<PrescriptionItem>(query, new { Id = id });
        return result;
    }

    public async Task<int> AddAsync(PrescriptionItem entity)
    {
        const string query = @"INSERT INTO [PrescriptionItem] (ListId, Title, Done, Note, CreatedBy, LastModifiedBy) 
                               OUTPUT INSERTED.Id
                               VALUES (@ListId, @Title, @Done, @Note, @CreatedBy, @CreatedBy);";
        var insertedId = await _connection.QuerySingleAsync<int>(query, entity);
        return insertedId;
    }

    public async Task<int> UpdateAsync(PrescriptionItem entity)
    {
        const string query = @"UPDATE [PrescriptionItem]
                                SET [Title] = @Title, [Done] = @Done, [LastModifiedBy] = @LastModifiedBy
                                WHERE Id = @Id";
        var affectedRows = await _connection.ExecuteAsync(query, entity);
        return affectedRows;
    }

    public async Task<int> UpdateDetailsAsync(PrescriptionItem entity)
    {
        const string query = @"UPDATE [PrescriptionItem]
                                SET ListId = @ListId, Priority = @Priority, Note = @Note
                                WHERE Id = @Id";
        var affectedRows = await _connection.ExecuteAsync(query, entity);
        return affectedRows;
    }

   

    public async Task<IReadOnlyList<PrescriptionItem>> GetByPrescriptionListIdAsync(int prescriptionListId)
    {
        const string query = $"SELECT * FROM [PrescriptionItem] WHERE [ListId] = @ListId ORDER BY [Title] DESC";
        var results = (await _connection.QueryAsync<PrescriptionItem>(query, new { ListId = prescriptionListId })).ToList();
        return results;
    }

    public async Task<PrescriptionItem?> FindByTitleAsync(string title)
    {
        const string query = "SELECT * FROM [PrescriptionItem] WHERE [Title] = @Title";
        var result = await _connection.QueryFirstOrDefaultAsync<PrescriptionItem>(query, 
            new { Title = title });
        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        const string query = "DELETE FROM [PrescriptionItem] WHERE Id = @Id";
        var affectedRows = await _connection.ExecuteAsync(query, new { Id = id });
        return affectedRows;
    }

    public async Task<int> DeleteAllAsync()
    {
        const string query = "DELETE FROM [PrescriptionItem]";
        var affectedRows = await _connection.ExecuteAsync(query);
        return affectedRows;
    }
}