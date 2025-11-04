using System;
using System.IO;
using OpenHtmlToPdf;

namespace CarDealership.util;

public static class HtmlToPdfGenerator
{
    public static byte[] FromHtmlString(
        string html,
        string? baseUrl = null,
        PaperSize? size = null,
        bool landscape = false,
        double marginTopMm = 10,
        double marginRightMm = 10,
        double marginBottomMm = 10,
        double marginLeftMm = 10)
    {
        if (string.IsNullOrWhiteSpace(html))
            throw new ArgumentException("HTML is empty", nameof(html));

        var effectiveSize = size ?? PaperSize.A4;

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            try
            {
                var norm = baseUrl!;
                if (!norm.EndsWith(Path.DirectorySeparatorChar) && !norm.EndsWith("/"))
                    norm += Path.DirectorySeparatorChar;
                var baseHref = new Uri(norm).AbsoluteUri;
                html = html.Replace("<head>", $"<head><base href=\"{baseHref}\" />");
            }
            catch
            {
            }
        }

        var builder = Pdf
            .From(html)
            .OfSize(effectiveSize)
            .WithMargins(Math.Max(Math.Max(marginLeftMm, marginRightMm), Math.Max(marginTopMm, marginBottomMm))
                .Millimeters())
            .WithObjectSetting("web.enableIntelligentShrinking", "false")
            .WithObjectSetting("load.zoomFactor", "1.25");

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            builder = builder
                .WithObjectSetting("web.defaultEncoding", "utf-8")
                .WithGlobalSetting("enable-local-file-access", "true");
        }

        if (!string.IsNullOrWhiteSpace(baseUrl))
            builder = builder.WithObjectSetting("web.defaultEncoding", "utf-8")
                .WithGlobalSetting("documentTitle", "document");

        builder = landscape ? builder.Landscape() : builder.Portrait();

        return builder.Content();
    }
}