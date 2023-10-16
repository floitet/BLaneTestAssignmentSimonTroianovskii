using AutoMapper;
using AutoMapper.QueryableExtensions;
using BallastLaneTestAssignment.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BallastLaneTestAssignment.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IEnumerable<TDestination> enumerable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(enumerable.AsQueryable(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static Task<List<TDestination>> ProjectToListAsync<TSource, TDestination>(this IReadOnlyList<TSource>  enumerable,
        IConfigurationProvider configuration, CancellationToken cancellationToken) where TDestination : class
        => enumerable.AsQueryable().ProjectTo<TDestination>(configuration).ToListAsync(cancellationToken: cancellationToken);
}
