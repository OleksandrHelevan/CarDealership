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
                Client = ClientMapper.ToDto(e.Client),
                Product = ProductMapper.ToDto(e.Product),
                OrderDate = e.OrderDate,
                PaymentType = e.PaymentType,
                Delivery = e.Delivery
            };
        }

        public static Order ToEntity(OrderDto dto)
        {
            return new Order
            {
                Client = ClientMapper.ToEntity(dto.Client),
                Product = ProductMapper.ToEntity(dto.Product),
                OrderDate = dto.OrderDate,
                PaymentType = dto.PaymentType,
                Delivery = dto.Delivery
            };
        }
    }
}