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

        var order = (from o in _context.Orders
                     where o.Id == orderId
                     join p in _context.Products on o.ProductId equals p.Id
                     join c in _context.Cars on p.CarId equals c.Id
                     join cl in _context.Clients on o.ClientId equals cl.Id
                     join pd in _context.PassportData on cl.PassportDataId equals pd.Id
                     select new { o, p, c, pd }).FirstOrDefault();

        if (order == null)
            throw new InvalidOperationException("Замовлення не знайдено.");

        var last4 = new string(cardNumber.Where(char.IsDigit).TakeLast(4).ToArray());
        var carName = $"{order.c.Brand} {order.c.ModelName}";
        var buyer = $"{order.pd.FirstName} {order.pd.LastName}";
        var html = ReceiptTemplate.Build(order.o, order.p.Number, carName, order.c.Price, last4, buyer);
        var baseDir = AppContext.BaseDirectory;
        var pdf = HtmlToPdfGenerator.FromHtmlString(html, baseDir, OpenHtmlToPdf.PaperSize.A5);

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
