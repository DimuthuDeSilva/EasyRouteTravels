namespace LocalTourPlanner.Models
{
    public class LocationViewModel
    {
        public LocationViewModel() 
        { 
        Locations = new List<Location>();
        }
            public List<Location> Locations { get; set; }

    }
}
