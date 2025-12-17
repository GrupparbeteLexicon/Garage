namespace Garage.Models
{
    public class ParkedVehicle
    {
            public int Id { get; set; }
            public VehicleType VehicleType { get; set; }
            public string Registration { get; set; }
            public string Color { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public int Wheels { get; set; }
            public DateTime ParkTime { get; }
    }
}
