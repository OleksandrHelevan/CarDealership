using CarDealership.enums;

namespace CarDealership.dto
{
    public class BuyCarDto
    {
        public int CarId { get; set; }
        public CarType CarType { get; set; }
        public string CountryOfOrigin { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public int ClientId { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool Delivery { get; set; }
    }
}
