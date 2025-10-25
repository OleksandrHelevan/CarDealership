using System.IO;

using System;
using System.IO;

namespace CarDealership.util;

public static class EmailTemplateRenderer
{
    public static string RenderResetCodeTemplate(string templatePath, string code)
    {
        if (!File.Exists(templatePath))
        {
            return $"<html><body><h2>Код підтвердження</h2><p>Ваш код: <strong>{code}</strong></p></body></html>";
        }

        var html = File.ReadAllText(templatePath);
        html = html.Replace("{{CODE}}", code);
        html = html.Replace("{{ISSUED_AT}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        return html;
    }
}
