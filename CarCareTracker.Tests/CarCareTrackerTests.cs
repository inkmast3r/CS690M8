namespace CarCareTracker.Test;
using System.IO;
using CarCareTracker;

public class CarCareTrackerTests
{
    protected FileSaver testSaver;
    protected string testFile = $"test_car_data_{Guid.NewGuid()}.txt";

    public CarCareTrackerTests()
    {
        testSaver = new FileSaver(testFile);

        if (File.Exists(testFile))
        {
            File.Delete(testFile);
        }
    }
}