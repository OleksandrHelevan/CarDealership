using System;
using System.IO;
using OpenHtmlToPdf;

namespace CarDealership.util;

public static class HtmlToPdfGenerator
{
    public static byte[] FromHtmlString(
        string html,
        string? baseUrl = null,
        OpenHtmlToPdf.PaperSize? size = null,
        bool landscape = false,
        double marginTopMm = 10,
        double marginRightMm = 10,
        double marginBottomMm = 10,
        double marginLeftMm = 10)
    {
        if (string.IsNullOrWhiteSpace(html))
            throw new ArgumentException("HTML is empty", nameof(html));

        var effectiveSize = size ?? OpenHtmlToPdf.PaperSize.A4;

        var builder = Pdf
            .From(html)
            .OfSize(effectiveSize)
            // Uniform margins (mm)
            .WithMargins(Math.Max(Math.Max(marginLeftMm, marginRightMm), Math.Max(marginTopMm, marginBottomMm)).Millimeters())
            // Make sure the engine does not shrink content unexpectedly
            .WithObjectSetting("web.enableIntelligentShrinking", "false")
            // Slight zoom to improve readability
            .WithObjectSetting("load.zoomFactor", "1.25");

        if (!string.IsNullOrWhiteSpace(baseUrl))
            builder = builder.WithObjectSetting("web.defaultEncoding", "utf-8").WithGlobalSetting("documentTitle", "document");

        builder = landscape ? builder.Landscape() : builder.Portrait();

        return builder.Content();
    }

    // Convenience: save to path and return the path
    public static string SaveToFile(string html, string outputPath,
        string? baseUrl = null,
        OpenHtmlToPdf.PaperSize? size = null,
        bool landscape = false)
    {
        var bytes = FromHtmlString(html, baseUrl, size, landscape);
        var dir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllBytes(outputPath, bytes);
        return outputPath;
    }
}
