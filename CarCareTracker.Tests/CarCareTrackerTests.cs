namespace CarCareTracker.Test;
using System.IO;
using Xunit;
using CarCareTracker;

public class CarCareTrackerTests
{
    FileSaver testSaver;
    string testFile = "test_car_data.txt";

    public CarCareTrackerTests()
    {
        testSaver = new FileSaver(testFile);

        // Ensure test file starts clean
        if (File.Exists(testFile))
        {
            File.Delete(testFile);
        }
    }

    [Fact]
    public void Test_SaveAndLoad_FuelLog()
    {
        CarData data = new CarData();

        data.FuelLogs.Add(new FuelLog
        {
            Date = "2026-03-05",
            Gallons = 20,
            Cost = 40.00,
            Mileage = 52000
        });

        testSaver.SaveData(data);

        CarData loaded = testSaver.LoadData();

        Assert.Single(loaded.FuelLogs);
        Assert.Equal(20, loaded.FuelLogs[0].Gallons);
        Assert.Equal(40.00, loaded.FuelLogs[0].Cost);
        Assert.Equal(52000, loaded.FuelLogs[0].Mileage);
    }

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

    [Fact]
    public void Test_SaveAndLoad_Issue()
    {
        CarData data = new CarData();

        data.Issues.Add(new Issue
        {
            Date = "2026-03-10",
            Description = "Check engine light"
        });

        testSaver.SaveData(data);

        CarData loaded = testSaver.LoadData();

        Assert.Single(loaded.Issues);
        Assert.Equal("Check engine light", loaded.Issues[0].Description);
    }

    [Fact]
    public void Test_SaveAndLoad_Insurance()
    {
        CarData data = new CarData();

        data.Insurance = new Insurance
        {
            Provider = "State Farm",
            RenewalDate = "2026-08-01"
        };

        testSaver.SaveData(data);

        CarData loaded = testSaver.LoadData();

        Assert.NotNull(loaded.Insurance);
        Assert.Equal("State Farm", loaded.Insurance.Provider);
        Assert.Equal("2026-08-01", loaded.Insurance.RenewalDate);
    }
}
