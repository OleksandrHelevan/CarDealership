using CarDealership.enums;

namespace CarDealership.model
{
    public class Bus : Vehicle
    {
        public int SeatingCapacity { get; set; }
        public int StandingCapacity { get; set; }

        public Bus(
            string brand,
            string modelName,
            Engine engine,
            Color color,
            int mileage,
            double price,
            int weight,
            int seatingCapacity,
            int standingCapacity
        ) : base(brand, modelName, engine, color, mileage, price, weight)
        {
            SeatingCapacity = seatingCapacity;
            StandingCapacity = standingCapacity;
        }

        public override string Print()
        {
            return $"🚌 Bus: {Brand} {ModelName}, {Engine}, {Color}, {Mileage} km, ${Price}, {Weight} kg, " +
                   $"Seats: {SeatingCapacity}, Standing: {StandingCapacity}";
        }
    }
}