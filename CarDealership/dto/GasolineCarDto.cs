using CarDealership.enums;

namespace CarDealership.dto
{
    public class GasolineCarDto : Vehicle
    {
        public int Id { get; set; }
        public DriveType DriveType { get; set; }
        public TransmissionType Transmission { get; set; }

        public string DriveTypeString { get; set; }
        public string TransmissionString { get; set; }

        public GasolineCarDto()
        {
        }

        public GasolineCarDto(
            string brand,
            string modelName,
            GasolineEngineDto engine,
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
            DriveTypeString = DriveType.ToFriendlyString();
            DriveType = drive;
            Transmission = transmission;
            TransmissionString = Transmission.ToFriendlyString();
        }
        
    }
}