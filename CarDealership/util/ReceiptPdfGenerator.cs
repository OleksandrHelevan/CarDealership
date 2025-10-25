using System;
using System.IO;
using CarDealership.entity;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarDealership.util;

public static class ReceiptPdfGenerator
{
    public static byte[] Generate(Order order, string productNumber, string carName, decimal price, string cardLast4)
    {
        // Ensure QuestPDF license configured (Community license)
        QuestPDF.Settings.License = LicenseType.Community;
        byte[] result;
        var now = DateTime.UtcNow;

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text("Квитанція про покупку").FontSize(20).SemiBold();
                    col.Item().Text($"Номер замовлення: {order.Id}");
                    col.Item().Text($"Товар: {carName} ({productNumber})");
                    col.Item().Text($"Сума: {price:C}");
                    col.Item().Text($"Оплата карткою •••• {cardLast4}");
                    col.Item().Text($"Дата: {now:yyyy-MM-dd HH:mm} UTC");
                });
            });
        });

        using var ms = new MemoryStream();
        doc.GeneratePdf(ms);
        result = ms.ToArray();
        return result;
    }
}
