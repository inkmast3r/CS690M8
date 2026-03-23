namespace CarCareTracker;
using System;
using System.Linq;

class CarCareTracker
{
    public static void Run()
    {
        FileSaver fileSaver = new FileSaver("car_data.txt");
        CarData data = fileSaver.LoadData();

        while (true)
        {
            Console.WriteLine("\n--- Car Care Tracker ---");
            Console.WriteLine("1. Add Fuel Log");
            Console.WriteLine("2. View Fuel Summary");
            Console.WriteLine("3. Add Maintenance Record");
            Console.WriteLine("4. Check Maintenance Reminders");
            Console.WriteLine("5. Log Car Issue");
            Console.WriteLine("6. View Logged Issues");
            Console.WriteLine("7. Set Insurance Info");
            Console.WriteLine("8. Check Insurance Status");
            Console.WriteLine("9. Save & Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1": AddFuelLog(data); 
                break;

                case "2": FuelSummary(data); 
                break;

                case "3": AddMaintenance(data); 
                break;

                case "4": CheckMaintenanceReminders(data); 
                break;

                case "5": LogIssue(data); 
                break;

                case "6": ViewIssues(data); 
                break;

                case "7": SetInsurance(data); 
                break;

                case "8": CheckInsurance(data); 
                break;

                case "9":
                    fileSaver.SaveData(data);
                    Console.WriteLine("Data saved. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    // fuel

    static void AddFuelLog(CarData data)
    {
        Console.Write("Date (YYYY-MM-DD): ");
        string date = Console.ReadLine() ?? "";

        Console.Write("Gallons filled: ");
        double gallons = double.Parse(Console.ReadLine() ?? "0");

        Console.Write("Total cost: ");
        double cost = double.Parse(Console.ReadLine() ?? "0");

        Console.Write("Current mileage: ");
        int mileage = int.Parse(Console.ReadLine() ?? "0");

        data.FuelLogs.Add(new FuelLog
        {
            Date = date,
            Gallons = gallons,
            Cost = cost,
            Mileage = mileage
        });

        Console.WriteLine("Fuel log added.");
    }

    static void FuelSummary(CarData data)
    {
        if (data.FuelLogs.Count < 1)
        {
            Console.WriteLine("Not enough data for summary.");
            return;
        }

        double totalCost = data.FuelLogs.Sum(x => x.Cost);
        double totalGallons = data.FuelLogs.Sum(x => x.Gallons);

        int milesDriven = data.FuelLogs.Last().Mileage - data.FuelLogs.First().Mileage;
        double mpg = milesDriven / totalGallons;

        Console.WriteLine($"\nTotal Fuel Cost: ${totalCost:F2}");
        Console.WriteLine($"Total Gallons Used: {totalGallons:F2}");
        Console.WriteLine($"Estimated MPG: {mpg:F2}");
    }

    // maintenance

    static void AddMaintenance(CarData data)
    {
        Console.Write("Service type (Oil Change, Tires, etc.): ");
        string service = Console.ReadLine() ?? "";

        Console.Write("Date (YYYY-MM-DD): ");
        string date = Console.ReadLine() ?? "";

        Console.Write("Mileage at service: ");
        int mileage = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Service interval miles: ");
        int interval = int.Parse(Console.ReadLine() ?? "0");

        data.MaintenanceLogs.Add(new MaintenanceLog
        {
            ServiceType = service,
            Date = date,
            Mileage = mileage,
            IntervalMiles = interval
        });

        Console.WriteLine("Maintenance record added.");
    }

    static void CheckMaintenanceReminders(CarData data)
    {
        Console.Write("Enter current mileage: ");
        int currentMileage = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("\nMaintenance Reminders:");

        foreach (var record in data.MaintenanceLogs)
        {
            int nextDue = record.Mileage + record.IntervalMiles;

            if (currentMileage >= nextDue)
                Console.WriteLine($"{record.ServiceType} is OVERDUE!");
            else if (currentMileage >= nextDue - 500)
                Console.WriteLine($"{record.ServiceType} due within 500 miles.");
        }
    }

    // issues

    static void LogIssue(CarData data)
    {
        Console.Write("Describe the issue: ");
        string description = Console.ReadLine() ?? "";

        data.Issues.Add(new Issue
        {
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            Description = description
        });

        Console.WriteLine("Issue logged.");
    }

    static void ViewIssues(CarData data)
    {
        if (data.Issues.Count == 0)
        {
            Console.WriteLine("No issues logged.");
            return;
        }

        Console.WriteLine("\nLogged Issues:");
        foreach (var issue in data.Issues)
            Console.WriteLine($"{issue.Date}: {issue.Description}");
    }

    // insurance

    static void SetInsurance(CarData data)
    {
        Console.Write("Insurance provider: ");
        string provider = Console.ReadLine() ?? "";

        Console.Write("Renewal date (YYYY-MM-DD): ");
        string renewal = Console.ReadLine() ?? "";

        data.Insurance = new Insurance { Provider = provider, RenewalDate = renewal };

        Console.WriteLine("Insurance info saved.");
    }

    static void CheckInsurance(CarData data)
    {
        if (data.Insurance == null)
        {
            Console.WriteLine("No insurance information found.");
            return;
        }

        DateTime renewal = DateTime.Parse(data.Insurance.RenewalDate);
        int daysLeft = (renewal - DateTime.Today).Days;

        if (daysLeft < 0)
            Console.WriteLine("Insurance renewal is OVERDUE!");
        else if (daysLeft <= 30)
            Console.WriteLine($"Insurance renewal due in {daysLeft} days.");
        else
            Console.WriteLine($"Insurance renewal in {daysLeft} days.");
    }
}
