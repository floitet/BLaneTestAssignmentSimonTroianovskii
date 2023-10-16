using System.Data.Common;
using AutoMapper;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Infrastructure.Persistence.Mappings;
using BallastLaneTestAssignment.Infrastructure.Persistence.Models;
using Dapper;

namespace BallastLaneTestAssignment.Infrastructure.Persistence.Repositories;

public class PrescriptionListRepository : IPrescriptionListRepository
{
    private readonly DbConnection _connection;
    private IMapper _mapper;

    public PrescriptionListRepository(DbConnection connection, IMapper mapper)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PrescriptionList>> GetAllAsync()
    {
       
        var results = (await GetAllAsyncJoin()).ToList();
        
        if(results.Count == 0)
            results = (await GetAllAsyncNoJoin()).ToList();
        
        var mappedResults = 
            results.Select(p => p.ToPrescriptionList());
        return mappedResults.AsList();
    }
    
    public async Task<PrescriptionList?> GetByIdAsync(int id)
    {
        const string query = "SELECT * FROM [PrescriptionList] WHERE ListId = @Id";
        var result = await _connection.QueryFirstOrDefaultAsync<PrescriptionListFromDb>(query, new { Id = id });
        var mappedResult = result?.ToPrescriptionList();
        return mappedResult;
    }

    public async Task<int> AddAsync(PrescriptionList entity)
    {
        const string query = @"INSERT INTO [PrescriptionList] (Title, ColourCode, CreatedBy, LastModifiedBy) 
                               OUTPUT INSERTED.ListId
                               VALUES (@Title, @Colour, @CreatedBy, @CreatedBy);";
        var insertedId = await _connection.QuerySingleAsync<int>(query, 
            new { entity.Title, Colour = entity.Colour.ToString(), entity.CreatedBy });
        return insertedId;
    }

    public async Task<int> UpdateAsync(PrescriptionList entity)
    {
        const string query = @"UPDATE [PrescriptionList]
                                    SET Title = @Title, LastModifiedBy = @LastModifiedBy 
                                    WHERE ListId = @ListId";
        var affectedRows = await _connection.ExecuteAsync(query, entity);
        return affectedRows;
    }

    
    public async Task<int> DeleteAsync(int id)
    {
        const string query = "DELETE FROM [PrescriptionList] WHERE ListId = @ListId";
        var affectedRows = await _connection.ExecuteAsync(query, new { ListId = id });
        return affectedRows;
    }

    public async Task<int> DeleteAllAsync()
    {
        const string query = "DELETE FROM [PrescriptionList]";
        var affectedRows = await _connection.ExecuteAsync(query);
        return affectedRows;
    }
    
    public async Task<int> GetPrescriptionListCountByTitle(string title)
    {
        const string query = "SELECT count(*) FROM [PrescriptionList] WHERE [Title] = @Title";
        var result = await _connection.QueryFirstOrDefaultAsync<int>(query, new { Title = title });
        return result;
    }
    
    private async Task<IReadOnlyList<PrescriptionListFromDb>> GetAllAsyncJoin()
    {
        const string query = "SELECT pl.*, pi.* " +
                             "FROM [PrescriptionList] as pl inner join [PrescriptionItem] as pi " +
                             "on pl.[ListId] = pi.[ListId];";
        
        var results = (await _connection.QueryAsync<PrescriptionListFromDb, PrescriptionItem, PrescriptionListFromDb>(query,
            (pList, pItem) =>
            {
                pList.Items.Add(pItem);
                return pList;
            }, splitOn: "ListId"))
            .GroupBy(p => p.ListId) // Group by ListId
            .Select(group =>
            {
                var prescriptionList = group.First(); // Get the first item in the group
                var items = group.Select(p => p.Items).SelectMany(i => i).ToList();
                prescriptionList.Items = items;
                return prescriptionList;
            })
            .ToList();;
        
        return results;
    }
    
    private async Task<IReadOnlyList<PrescriptionListFromDb>> GetAllAsyncNoJoin()
    {
        const string query = "SELECT * FROM [PrescriptionList]";
        var results = (await _connection.QueryAsync<PrescriptionListFromDb>(query)).ToList();
        return results;
    }
}