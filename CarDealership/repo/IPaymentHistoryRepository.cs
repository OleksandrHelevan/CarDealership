using CarDealership.entity;

namespace CarDealership.repo;

public interface IPaymentHistoryRepository
{
    void Add(PaymentHistory history);
}