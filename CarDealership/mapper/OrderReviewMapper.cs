using CarDealership.dto;
using CarDealership.entity;

namespace CarDealership.mapper;

public static class OrderReviewMapper
{
    public static OrderReviewDto ToDto(OrderReview e)
    {
        return new OrderReviewDto
        {
            Id = e.Id,
            OrderId = e.OrderId,
            Status = e.Status,
            Message = e.Message,
            RequiresDeliveryAddress = e.RequiresDeliveryAddress,
            RequiresCardNumber = e.RequiresCardNumber,
            CardNumber = e.CardNumber,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}