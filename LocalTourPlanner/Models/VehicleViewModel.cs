namespace LocalTourPlanner.Models
{
    public class VehicleViewModel
    {
        public VehicleViewModel() 
        { 
        Vehicles = new List<Vehicle>();
        }
            public List<Vehicle> Vehicles { get; set; }

    }
}
