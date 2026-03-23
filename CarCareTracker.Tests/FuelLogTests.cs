namespace CarCareTracker.Test;
using Xunit;
using CarCareTracker;

public class FuelLogTests : CarCareTrackerTests
{
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
}