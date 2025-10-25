using CarDealership.dto;

namespace CarDealership.service;

public interface IOrderReviewService
{
    // Operator actions
    OrderReviewDto CreateForOrder(int orderId, string? message = null);
    void Approve(int reviewId, string? message = null);
    void Reject(int reviewId, string reason);

    // User actions (after operator approval)
    void SubmitDetails(int reviewId, string? deliveryAddress, string? cardNumber);

    // Queries
    OrderReviewDto? GetById(int id);
    OrderReviewDto? GetByOrderId(int orderId);
    List<OrderReviewDto> GetAll();
    List<OrderReviewDto> GetPending();
    List<OrderReviewDto> GetByClientId(int clientId);
}

