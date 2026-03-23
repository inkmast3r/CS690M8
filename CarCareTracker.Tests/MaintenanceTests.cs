namespace CarCareTracker.Test;
using Xunit;
using CarCareTracker;

public class MaintenanceTests : CarCareTrackerTests
{
    [Fact]
    public void Test_SaveAndLoad_Maintenance()
    {
        CarData data = new CarData();

        data.MaintenanceLogs.Add(new MaintenanceLog
        {
            ServiceType = "Oil Change",
            Date = "2026-01-23",
            Mileage = 51000,
            IntervalMiles = 3000
        });

        testSaver.SaveData(data);

        CarData loaded = testSaver.LoadData();

        Assert.Single(loaded.MaintenanceLogs);
        Assert.Equal("Oil Change", loaded.MaintenanceLogs[0].ServiceType);
        Assert.Equal(51000, loaded.MaintenanceLogs[0].Mileage);
        Assert.Equal(3000, loaded.MaintenanceLogs[0].IntervalMiles);
    }
}