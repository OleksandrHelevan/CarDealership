using CarDealership.enums;

namespace CarDealership.model
{
    public class CarDto : Vehicle
    {
        public DriveType Drive { get; set; }
        public TransmissionType Transmission { get; set; }

        public CarDto()
        {
            
        }
        public CarDto(
            string brand,
            string modelName,
            Engine engine,
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
            Drive = drive;
            Transmission = transmission;
        }

        public override string Print()
        {
            return
                $"Car: {Brand} {ModelName}, {Engine}, {Color}, {Mileage} km, ${Price}, {Weight} kg, {Transmission}, {Drive}";
        }
    }
}