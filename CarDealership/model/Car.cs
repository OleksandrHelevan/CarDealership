using CarDealership.enums;

namespace CarDealership.model
{
    public class Car : Vehicle
    {
        public DriveType Drive { get; set; }
        public TransmissionType Transmission { get; set; }

        public Car(
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
            return $"{Drive} drive, {Transmission} transmission";
        }
    }
}