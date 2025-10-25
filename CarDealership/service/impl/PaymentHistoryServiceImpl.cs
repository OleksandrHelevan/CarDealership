using System;
using System.Linq;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.repo;
using CarDealership.repo.impl;
using CarDealership.util;

namespace CarDealership.service.impl;

public class PaymentHistoryServiceImpl : IPaymentHistoryService
{
    private readonly DealershipContext _context;
    private readonly IPaymentHistoryRepository _repo;

    public PaymentHistoryServiceImpl(DealershipContext context, IPaymentHistoryRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public int CreateReceipt(int orderId, string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Count(char.IsDigit) < 12)
            throw new ArgumentException("Некоректний номер картки.");

        var order = _context.Orders
            .Where(o => o.Id == orderId)
            .Join(_context.Products, o => o.ProductId, p => p.Id, (o, p) => new { o, p })
            .Join(_context.Cars, op => op.p.CarId, c => c.Id, (op, c) => new { op.o, op.p, c })
            .Select(x => new { x.o, x.p, x.c })
            .FirstOrDefault();

        if (order == null)
            throw new InvalidOperationException("Замовлення не знайдено.");

        var last4 = new string(cardNumber.Where(char.IsDigit).TakeLast(4).ToArray());
        var carName = $"{order.c.Brand} {order.c.ModelName}";
        var html = ReceiptTemplate.Build(order.o, order.p.Number, carName, order.c.Price, last4);
        var pdf = HtmlToPdfGenerator.FromHtmlString(html);

        var history = new PaymentHistory
        {
            OrderId = orderId,
            Amount = order.c.Price,
            CardLast4 = last4,
            CreatedAt = DateTime.UtcNow,
            ReceiptPdf = pdf,
            ContentType = "application/pdf"
        };

        _repo.Add(history);
        return history.Id;
    }
}
