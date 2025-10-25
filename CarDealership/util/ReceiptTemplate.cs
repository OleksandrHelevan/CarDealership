using System;
using System.IO;
using CarDealership.entity;

namespace CarDealership.util;

public static class ReceiptTemplate
{
    private const string TemplateRelativePath = "templates/receipt.html";

    public static string Build(Order order, string productNumber, string carName, decimal price, string cardLast4, string customerFullName)
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, TemplateRelativePath);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Receipt template not found at {path}");

        var html = File.ReadAllText(path);
        html = html
            .Replace("{{OrderId}}", order.Id.ToString())
            .Replace("{{ProductNumber}}", productNumber)
            .Replace("{{CarName}}", carName)
            .Replace("{{CardLast4}}", cardLast4)
            .Replace("{{Date}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"))
            .Replace("{{Price}}", price.ToString("C"))
            .Replace("{{CustomerName}}", customerFullName);
        return html;
    }
}
