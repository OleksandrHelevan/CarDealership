using System;
using System.Net;
using System.Net.Mail;
using CarDealership.util;

namespace CarDealership.service.impl;

public class EmailServiceImpl : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _from;
    private readonly string _username;
    private readonly string _password;

    public EmailServiceImpl()
    {
        // Load .env (if present) to populate environment variables
        DotEnv.Load();

        _host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.example.com";
        _port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 587;
        _enableSsl = (Environment.GetEnvironmentVariable("SMTP_SSL") ?? "true").ToLowerInvariant() != "false";
        _from = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "no-reply@example.com";
        _username = Environment.GetEnvironmentVariable("SMTP_USER") ?? "";
        _password = Environment.GetEnvironmentVariable("SMTP_PASS") ?? "";
    }

    public void SendHtmlEmail(string to, string subject, string htmlBody)
    {
        using var smtp = new SmtpClient(_host, _port)
        {
            EnableSsl = _enableSsl,
            Credentials = string.IsNullOrEmpty(_username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(_username, _password)
        };

        using var message = new MailMessage(_from, to)
        {
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        smtp.Send(message);
    }
}
