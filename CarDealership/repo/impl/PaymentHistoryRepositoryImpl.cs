using CarDealership.config;
using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl;

public class PaymentHistoryRepositoryImpl : IPaymentHistoryRepository
{
    private readonly DealershipContext _context;
    public PaymentHistoryRepositoryImpl(DealershipContext context)
    {
        _context = context;
    }

    public void Add(PaymentHistory history)
    {
        try
        {
            _context.PaymentHistory.Add(history);
            _context.SaveChanges();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            var root = ex.InnerException?.Message ?? ex.Message;
            throw new InvalidOperationException($"Помилка збереження платежу: {root}", ex);
        }
    }

    public PaymentHistory? GetById(int id)
    {
        return _context.PaymentHistory
            .Include(p => p.Order)
            .ThenInclude(o => o.Product)
            .FirstOrDefault(p => p.Id == id);
    }

    public PaymentHistory? GetByOrderId(int orderId)
    {
        return _context.PaymentHistory
            .Include(p => p.Order)
            .ThenInclude(o => o.Product)
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.Id)
            .FirstOrDefault();
    }

    public List<PaymentHistory> GetByClientId(int clientId)
    {
        return _context.PaymentHistory
            .Include(p => p.Order)
            .Where(p => p.Order.ClientId == clientId)
            .ToList();
    }
}
