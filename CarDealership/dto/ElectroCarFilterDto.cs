using CarDealership.enums;

namespace CarDealership.dto
{
    public class ElectroCarFilterDto
    {
        public int? Id { get; set; }
        public string? SearchText { get; set; }
        public TransmissionType? TransmissionType { get; set; }
        public CarBodyType? BodyType { get; set; }
        public Color? Color { get; set; }
        public DriveType? DriveType { get; set; }
        public ElectroMotorType? MotorType { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }
        public float? WeightFrom { get; set; }
        public float? WeightTo { get; set; }
        public int? MileageFrom { get; set; }
        public int? MileageTo { get; set; }
        public int? DoorsFrom { get; set; }
        public int? DoorsTo { get; set; }
    }
}