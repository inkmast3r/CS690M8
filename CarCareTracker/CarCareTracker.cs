namespace CarCareTracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class CarCareTracker
{
    static void Main()
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

public class FileSaver
{
    private string fileName;

    public FileSaver(string fileName)
    {
        this.fileName = fileName;
    }

public CarData LoadData()
    {
        CarData data = new CarData();

        if (!File.Exists(fileName))
            return data;

        foreach (var line in File.ReadAllLines(fileName))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string trimmed = line.Trim();

            // only process real data
            if (trimmed.StartsWith("FUEL|"))
            {
                var parts = trimmed.Split('|');

                data.FuelLogs.Add(new FuelLog
                {
                    Date = parts[1].Trim(),
                    Gallons = double.Parse(parts[2].Replace("G", "").Trim()),
                    Cost = double.Parse(parts[3].Replace("$", "").Trim()),
                    Mileage = int.Parse(parts[4].Replace("mi", "").Trim())
                });
            }
            else if (trimmed.StartsWith("MAINTENANCE|"))
            {
                var parts = trimmed.Split('|');

                data.MaintenanceLogs.Add(new MaintenanceLog
                {
                    ServiceType = parts[1].Trim(),
                    Date = parts[2].Trim(),
                    Mileage = int.Parse(parts[3].Replace("mi", "").Trim()),
                    IntervalMiles = int.Parse(parts[4].Replace("mi", "").Replace(".", "").Trim())
                });
            }
            else if (trimmed.StartsWith("ISSUE|"))
            {
                var parts = trimmed.Split('|');

                data.Issues.Add(new Issue
                {
                    Date = parts[1].Trim(),
                    Description = parts[2].Trim()
                });
            }
            else if (trimmed.StartsWith("INSURANCE|"))
            {
                var parts = trimmed.Split('|');

                data.Insurance = new Insurance
                {
                    Provider = parts[1].Trim(),
                    RenewalDate = parts[2].Trim()
                };
            }
        }

        return data;
    }

    public void SaveData(CarData data)
    {
        List<string> lines = new List<string>();

        lines.Add("* FUEL| Date | Gallons | Cost | Mileage");
        lines.Add("---------------------------------------");
        foreach (var fuel in data.FuelLogs)
            lines.Add($"FUEL| {fuel.Date} | {fuel.Gallons}G | ${fuel.Cost:F2} | {fuel.Mileage} mi");
        lines.Add("                                                                                 ");
        lines.Add("* MAINTENANCE| Service | Date | Mileage | Interval");
        lines.Add("-------------------------------------------------");
        foreach (var m in data.MaintenanceLogs)
            lines.Add($"MAINTENANCE| {m.ServiceType} | {m.Date} | {m.Mileage} mi | {m.IntervalMiles} mi.");
        lines.Add("                                                                                 ");
        lines.Add("* ISSUE| Date | Description");
        lines.Add("---------------------------");        
        foreach (var issue in data.Issues)
            lines.Add($"ISSUE| {issue.Date} | {issue.Description}");
        lines.Add("                                                                                 ");
        lines.Add("* INSURANCE| Provider | RenewalDate");
        lines.Add("-----------------------------------");
        if (data.Insurance != null)
            lines.Add($"INSURANCE| {data.Insurance.Provider} | {data.Insurance.RenewalDate}");

        File.WriteAllLines(fileName, lines);
    }
}

// data classes

public class CarData
{
    public List<FuelLog> FuelLogs { get; set; } = new();
    public List<MaintenanceLog> MaintenanceLogs { get; set; } = new();
    public List<Issue> Issues { get; set; } = new();
    public Insurance Insurance { get; set; } = new Insurance();
}

public class FuelLog
{
    public string Date { get; set; } = "";
    public double Gallons { get; set; }
    public double Cost { get; set; }
    public int Mileage { get; set; }
}

public class MaintenanceLog
{
    public string ServiceType { get; set; } = "";
    public string Date { get; set; } = "";
    public int Mileage { get; set; }
    public int IntervalMiles { get; set; }
}

public class Issue
{
    public string Date { get; set; } = "";
    public string Description { get; set; } = "";
}

public class Insurance
{
    public string Provider { get; set; } = "";
    public string RenewalDate { get; set; } = "";
}