using LocalTourPlanner.Domain;
using System.Collections.Generic;

namespace LocalTourPlanner.Models
{
    public class CostBreakdownViewModel
    {
        // The list of locations selected by the user for the itinerary
        public List<TourPlanModel> SelectedLocations { get; set; } = new List<TourPlanModel>();

        // Details of the vehicle selected for transport
        public string SelectedVehicleName { get; set; } = string.Empty;

        // The per-kilometer rate defined by the admin for that vehicle
        public double RatePerKm { get; set; }

        // The total distance including the return trip to the hub
        public double TotalDistance { get; set; }

        // The final calculated price (Distance * Rate)
        public double GrandTotal { get; set; }

        // Formatted date for the PDF header
        public string GeneratedDate { get; set; } = string.Empty;
    }
}