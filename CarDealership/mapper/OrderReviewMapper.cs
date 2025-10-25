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

    public static OrderReview ToEntity(OrderReviewDto dto)
    {
        return new OrderReview
        {
            Id = dto.Id,
            OrderId = dto.OrderId,
            Status = dto.Status,
            Message = dto.Message,
            RequiresDeliveryAddress = dto.RequiresDeliveryAddress,
            RequiresCardNumber = dto.RequiresCardNumber,
            CardNumber = dto.CardNumber,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
