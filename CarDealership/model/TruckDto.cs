using CarDealership.enums;

namespace CarDealership.model
{
    public class TruckDto : Vehicle
    {
        public double CargoCapacity { get; set; }

        public TruckDto(
            string brand,
            string modelName,
            Engine engine,
            Color color,
            int mileage,
            double price,
            int weight,
            double cargoCapacity
        ) : base(brand, modelName, engine, color, mileage, price, weight)
        {
            CargoCapacity = cargoCapacity;
        }

        public override string Print()
        {
            return $"🚚 Truck: {Brand} {ModelName}, {Engine}, {Color}, {Mileage} km, ${Price}, {Weight} kg, Cargo: {CargoCapacity} kg";
        }
    }
}