namespace CarDealership.service;

public interface IPaymentHistoryService
{
    int CreateReceipt(int orderId, string cardNumber);
}

