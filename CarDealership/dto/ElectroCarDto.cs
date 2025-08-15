using CarDealership.enums;

namespace CarDealership.dto
{
    public class ElectroCarDto : Vehicle
    {
        public DriveType DriveType { get; set; }
        public TransmissionType Transmission { get; set; }
        
        public string DriveTypeString { get; set; }
        public string TransmissionString { get; set; }

        public ElectroCarDto()
        {
        }

        public ElectroCarDto(
            string brand,
            string modelName,
            ElectroEngineDto engine,
            Color color,
            int mileage,
            double price,
            int weight,
            DriveType drive,
            TransmissionType transmission,
            int year,
            int numberOfDoors,
            CarBodyType bodyType
        ) : base(brand, modelName, engine, color, mileage, price, weight, year, numberOfDoors, bodyType)
        {
            DriveType = drive;
            Transmission = transmission;
            TransmissionString = transmission.ToFriendlyString();
            DriveTypeString = DriveType.ToFriendlyString();
        }
    }
}