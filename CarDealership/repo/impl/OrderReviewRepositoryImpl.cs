using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl;

public class OrderReviewRepositoryImpl : IOrderReviewRepository
{
    private readonly DealershipContext _context;

    public OrderReviewRepositoryImpl(DealershipContext context)
    {
        _context = context;
    }

    public void Add(OrderReview review)
    {
        _context.OrderReviews.Add(review);
        _context.SaveChanges();
    }

    public void Update(OrderReview review)
    {
        _context.OrderReviews.Update(review);
        _context.SaveChanges();
    }

    public void Delete(OrderReview review)
    {
        _context.OrderReviews.Remove(review);
        _context.SaveChanges();
    }

    public OrderReview? GetById(int id)
    {
        return _context.OrderReviews
            .Include(r => r.Order)
                .ThenInclude(o => o.Client)
            .Include(r => r.Order)
                .ThenInclude(o => o.Product)
            .FirstOrDefault(r => r.Id == id);
    }

    public OrderReview? GetByOrderId(int orderId)
    {
        return _context.OrderReviews
            .Include(r => r.Order)
                .ThenInclude(o => o.Client)
            .Include(r => r.Order)
                .ThenInclude(o => o.Product)
            .FirstOrDefault(r => r.OrderId == orderId);
    }

    public List<OrderReview> GetAll()
    {
        return _context.OrderReviews
            .Include(r => r.Order)
                .ThenInclude(o => o.Client)
            .Include(r => r.Order)
                .ThenInclude(o => o.Product)
            .ToList();
    }

    public List<OrderReview> GetPendingWithDetails()
    {
        return _context.OrderReviews
            .Include(r => r.Order)
                .ThenInclude(o => o.Client)
            .Include(r => r.Order)
                .ThenInclude(o => o.Product)
            .Where(r => r.Status == RequestStatus.Pending)
            .ToList();
    }

    public List<OrderReview> GetByClientId(int clientId)
    {
        return _context.OrderReviews
            .Include(r => r.Order)
                .ThenInclude(o => o.Client)
            .Include(r => r.Order)
                .ThenInclude(o => o.Product)
            .Where(r => r.Order.ClientId == clientId)
            .ToList();
    }
}

