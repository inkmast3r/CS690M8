namespace CarCareTracker;

public class MaintenanceLog
{
    public string ServiceType { get; set; } = "";
    public string Date { get; set; } = "";
    public int Mileage { get; set; }
    public int IntervalMiles { get; set; }
}