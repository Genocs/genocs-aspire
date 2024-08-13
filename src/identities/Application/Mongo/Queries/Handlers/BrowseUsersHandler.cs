using Genocs.Core.CQRS.Queries;
using Genocs.Persistence.MongoDb.Repositories;
using GenocsAspire.Identities.Application.DTO;
using GenocsAspire.Identities.Application.Mongo.Documents;
using GenocsAspire.Identities.Application.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GenocsAspire.Identities.Application.Mongo.Queries.Handlers;

public class BrowseUsersHandler : IQueryHandler<BrowseUsers, PagedDto<UserDto>>
{
    private readonly IMongoDatabase _database;

    public BrowseUsersHandler(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<PagedDto<UserDto>> HandleAsync(BrowseUsers query, CancellationToken cancellationToken = default)
    {
        var result = await _database.GetCollection<UserDocument>("users")
            .AsQueryable()
            .OrderByDescending(x => x.CreatedAt)
            .PaginateAsync(query);

        var pagedResult = PagedResult<UserDto>.From(result, result.Items.Select(x => Map(x)));
        return new PagedDto<UserDto>
        {
            CurrentPage = pagedResult.CurrentPage,
            TotalPages = pagedResult.TotalPages,
            ResultsPerPage = pagedResult.ResultsPerPage,
            TotalResults = pagedResult.TotalResults,
            Items = pagedResult.Items
        };
    }

    private static UserDto Map(UserDocument user)
        => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            Locked = user.Locked
        };
}