namespace CarDealership.service;

public interface IEmailService
{
    void SendHtmlEmail(string to, string subject, string htmlBody);
}

