using Genocs.Core.CQRS.Queries;
using Genocs.Persistence.MongoDb.Repositories.Mentor;
using GenocsAspire.OrdersApiService.Domain;
using GenocsAspire.OrdersApiService.DTO;
using GenocsAspire.OrdersApiService.Queries;

namespace GenocsAspire.OrdersApiService.Queries.Handlers;

public class GetOrderHandler : IQueryHandler<GetOrder, OrderDto>
{
    private readonly IMongoRepository<Order, Guid> _repository;

    public GetOrderHandler(IMongoRepository<Order, Guid> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// GetOrder query handler.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<OrderDto?> HandleAsync(GetOrder query, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetAsync(query.OrderId);

        return order is null
            ? null
            : new OrderDto { Id = order.Id, CustomerId = order.CustomerId, TotalAmount = order.TotalAmount };
    }
}