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
                Client = ClientMapper.ToDto(e.Client),
                Product = ProductMapper.ToDto(e.Product),
                OrderDate = e.OrderDate,
                CreatedAt = e.OrderDate,
                PaymentType = e.PaymentType,
                Delivery = e.Delivery,
                Address = e.Address,
                PhoneNumber = e.PhoneNumber
            };
        }

        public static Order ToEntity(OrderDto dto)
        {
            return new Order
            {
                OrderDate = dto.OrderDate,
                PaymentType = dto.PaymentType,
                Delivery = dto.Delivery,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                ClientId = dto.Client.Id,
                ProductId = dto.Product.Id
            };
        }
    }
}
