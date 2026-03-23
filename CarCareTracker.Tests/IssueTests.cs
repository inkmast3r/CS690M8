namespace CarCareTracker.Test;
using Xunit;
using CarCareTracker;

public class IssueTests : CarCareTrackerTests
{
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
}