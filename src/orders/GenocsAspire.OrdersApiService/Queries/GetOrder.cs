using Genocs.Core.CQRS.Queries;
using GenocsAspire.OrdersApiService.DTO;
using System;

namespace GenocsAspire.OrdersApiService.Queries;

public class GetOrder : IQuery<OrderDto>
{
    public Guid OrderId { get; set; }
}