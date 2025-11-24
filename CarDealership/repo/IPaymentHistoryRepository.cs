using CarDealership.entity;

namespace CarDealership.repo;

public interface IPaymentHistoryRepository
{
    void Add(PaymentHistory history);
    PaymentHistory? GetById(int id);
    List<PaymentHistory> GetByClientId(int clientId);
    List<PaymentHistory> GetAll();
}