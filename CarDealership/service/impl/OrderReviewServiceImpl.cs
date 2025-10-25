using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.mapper;
using CarDealership.repo;
using CarDealership.repo.impl;

namespace CarDealership.service.impl;

public class OrderReviewServiceImpl : IOrderReviewService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReviewRepository _reviewRepository;

    public OrderReviewServiceImpl(IOrderRepository orderRepository, IOrderReviewRepository reviewRepository)
    {
        _orderRepository = orderRepository;
        _reviewRepository = reviewRepository;
    }

    public OrderReviewDto CreateForOrder(int orderId, string? message = null)
    {
        var order = _orderRepository.GetById(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order not found: {orderId}");

        var requiresDeliveryAddress = order.Delivery;
        var requiresCardNumber = order.PaymentType == PaymentType.Card;

        var review = new OrderReview
        {
            OrderId = orderId,
            Status = RequestStatus.Pending,
            Message = message,
            RequiresDeliveryAddress = requiresDeliveryAddress,
            RequiresCardNumber = requiresCardNumber,
            CreatedAt = DateTime.UtcNow
        };

        _reviewRepository.Add(review);
        return OrderReviewMapper.ToDto(review);
    }

    public void Approve(int reviewId, string? message = null)
    {
        var review = _reviewRepository.GetById(reviewId) ?? throw new InvalidOperationException("Review not found");
        review.Status = RequestStatus.Approved;
        review.Message = message;
        review.UpdatedAt = DateTime.UtcNow;
        _reviewRepository.Update(review);
    }

    public void Reject(int reviewId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required", nameof(reason));

        var review = _reviewRepository.GetById(reviewId) ?? throw new InvalidOperationException("Review not found");
        review.Status = RequestStatus.Rejected;
        review.Message = reason.Trim();
        review.UpdatedAt = DateTime.UtcNow;
        _reviewRepository.Update(review);
    }

    public void SubmitDetails(int reviewId, string? deliveryAddress, string? cardNumber)
    {
        var review = _reviewRepository.GetById(reviewId) ?? throw new InvalidOperationException("Review not found");

        if (review.RequiresDeliveryAddress && string.IsNullOrWhiteSpace(deliveryAddress))
            throw new ArgumentException("Delivery address is required for delivery orders", nameof(deliveryAddress));
        if (review.RequiresCardNumber && string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("Card number is required for card payments", nameof(cardNumber));

        if (review.RequiresDeliveryAddress)
        {
            // persist address on Order entity
            var order = review.Order;
            order.Address = deliveryAddress?.Trim();
            _orderRepository.Update(order);
        }
        if (review.RequiresCardNumber)
            review.CardNumber = cardNumber?.Trim();

        review.UpdatedAt = DateTime.UtcNow;
        _reviewRepository.Update(review);
    }

    public OrderReviewDto? GetById(int id)
    {
        var e = _reviewRepository.GetById(id);
        return e == null ? null : OrderReviewMapper.ToDto(e);
    }

    public OrderReviewDto? GetByOrderId(int orderId)
    {
        var e = _reviewRepository.GetByOrderId(orderId);
        return e == null ? null : OrderReviewMapper.ToDto(e);
    }

    public List<OrderReviewDto> GetAll()
    {
        return _reviewRepository.GetAll().Select(OrderReviewMapper.ToDto).ToList();
    }

    public List<OrderReviewDto> GetPending()
    {
        return _reviewRepository.GetPendingWithDetails().Select(OrderReviewMapper.ToDto).ToList();
    }

    public List<OrderReviewDto> GetByClientId(int clientId)
    {
        return _reviewRepository.GetByClientId(clientId).Select(OrderReviewMapper.ToDto).ToList();
    }
}
