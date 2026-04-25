namespace LocalTourPlanner.Models
{
    public class LocationViewModel
    {
        public LocationViewModel() 
        { 
        Locations = new List<LocationModel>();
        }
            public List<LocationModel> Locations { get; set; }

    }
}
