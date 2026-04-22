using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    [Key]
    public int VID { get; set; }
    public string VehicleName { get; set; }
    public string VehicleType { get; set; }
    public int Rate { get; set; }
    public int SeatingCapacity { get; set; }
}