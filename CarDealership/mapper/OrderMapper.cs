using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(Order e)
        {
            return new OrderDto
            {
                Id = e.Id,
                OrderId = e.Id,
                ClientId = e.ClientId,
                ProductId = e.ProductId,
                Client = ClientMapper.ToDto(e.Client),
                Product = ProductMapper.ToDto(e.Product),
                OrderDate = e.OrderDate,
                CreatedAt = e.CreatedAt,
                PaymentType = e.PaymentType,
                DeliveryRequired = e.DeliveryRequired,
                CarName = e.Product?.Car?.FullName ?? string.Empty
            };
        }

        public static Order ToEntity(OrderDto dto)
        {
            return new Order
            {
                OrderDate = dto.OrderDate,
                PaymentType = dto.PaymentType,
                DeliveryRequired = dto.DeliveryRequired,
                ClientId = dto.ClientId != 0 ? dto.ClientId : dto.Client.Id,
                ProductId = dto.ProductId != 0 ? dto.ProductId : dto.Product.Id
            };
        }
    }
}