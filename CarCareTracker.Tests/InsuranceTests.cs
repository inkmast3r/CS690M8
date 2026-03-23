namespace CarCareTracker.Test;
using Xunit;
using CarCareTracker;

public class InsuranceTests : CarCareTrackerTests
{
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