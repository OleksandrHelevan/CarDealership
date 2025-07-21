using CarDealership.enums;

namespace CarDealership.model
{
    public class CarDto : Vehicle
    {
        public DriveType Drive { get; set; }
        public TransmissionType Transmission { get; set; }

        public CarDto(
            string brand,
            string modelName,
            Engine engine,
            Color color,
            int mileage,
            double price,
            int weight,
            DriveType drive,
            TransmissionType transmission
        ) : base(brand, modelName, engine, color, mileage, price, weight)
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