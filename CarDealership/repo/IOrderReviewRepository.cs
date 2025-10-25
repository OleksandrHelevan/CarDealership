using CarDealership.entity;

namespace CarDealership.repo;

public interface IOrderReviewRepository
{
    void Add(OrderReview review);
    void Update(OrderReview review);
    void Delete(OrderReview review);
    OrderReview? GetById(int id);
    OrderReview? GetByOrderId(int orderId);
    List<OrderReview> GetAll();
    List<OrderReview> GetPendingWithDetails();
    List<OrderReview> GetByClientId(int clientId);
}

