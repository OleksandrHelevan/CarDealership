    using CarDealership.enums;

    namespace CarDealership.dto
    {
    public class BuyCarDto
    {
        public int Id { get; set; }
        public CarType CarType { get; set; }
        public string CountryOfOrigin { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public int ClientId { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool Delivery { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    }
