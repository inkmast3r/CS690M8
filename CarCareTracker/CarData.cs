namespace CarCareTracker;
using System.Collections.Generic;

public class CarData
{
    public List<FuelLog> FuelLogs { get; set; } = new();
    public List<MaintenanceLog> MaintenanceLogs { get; set; } = new();
    public List<Issue> Issues { get; set; } = new();
    public Insurance Insurance { get; set; } = new Insurance();
}