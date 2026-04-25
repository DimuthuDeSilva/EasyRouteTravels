namespace LocalTourPlanner.Models
{
    public class VehicleViewModel
    {
        public VehicleViewModel() 
        { 
            Vehicles = new List<VehicleModel>();
        }
            public List<VehicleModel> Vehicles { get; set; }

    }
}
