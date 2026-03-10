using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
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
            Console.WriteLine("5. Save & Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddFuelLog(data);
                    break;

                case "2":
                    FuelSummary(data);
                    break;

                case "3":
                    AddMaintenance(data);
                    break;

                case "4":
                    CheckMaintenanceReminders(data);
                    break;

                case "5":
                    fileSaver.SaveData(data);
                    Console.WriteLine("Data saved. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void AddFuelLog(CarData data)
    {
        Console.Write("Date (YYYY-MM-DD): ");
        string date = Console.ReadLine();

        Console.Write("Gallons filled: ");
        double gallons = double.Parse(Console.ReadLine());

        Console.Write("Total cost: ");
        double cost = double.Parse(Console.ReadLine());

        Console.Write("Current mileage: ");
        int mileage = int.Parse(Console.ReadLine());

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

    static void AddMaintenance(CarData data)
    {
        Console.Write("Service type (Oil Change, Tires, etc.): ");
        string service = Console.ReadLine();

        Console.Write("Date (YYYY-MM-DD): ");
        string date = Console.ReadLine();

        Console.Write("Mileage at service: ");
        int mileage = int.Parse(Console.ReadLine());

        Console.Write("Service interval miles: ");
        int interval = int.Parse(Console.ReadLine());

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
        int currentMileage = int.Parse(Console.ReadLine());

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
}

class FileSaver
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

        var lines = File.ReadAllLines(fileName);

        foreach (var line in lines)
        {
            if (line.StartsWith("#"))
                continue;

            var parts = line.Split('|');

            if (parts[0] == "FUEL")
            {
                data.FuelLogs.Add(new FuelLog
                {
                    Date = parts[1],
                    Gallons = double.Parse(parts[2].Replace("G","")),
                    Cost = double.Parse(parts[3].Replace("$","")),
                    Mileage = int.Parse(parts[4].Replace("mi","").Trim())
                });
            }

            if (parts[0] == "MAINTENANCE")
            {
                data.MaintenanceLogs.Add(new MaintenanceLog
                {
                    ServiceType = parts[1],
                    Date = parts[2],
                    Mileage = int.Parse(parts[3].Replace("mi", "").Replace(".", "").Trim()),
                    IntervalMiles = int.Parse(parts[4].Replace("mi", "").Replace(".", "").Trim())
                });
            }
        }

        return data;
    }

    public void SaveData(CarData data)
    {
        List<string> lines = new List<string>();

        // Header lines
        lines.Add("# Fuel Format: FUEL | Date | Gallons | Cost | Mileage");
        lines.Add("# Maintenance Format: MAINTENANCE| Service Type | Date | Mileage | Interval Miles");
        lines.Add("-------------------------------------------------------------------------");

        foreach (var fuel in data.FuelLogs)
        {
           lines.Add($"FUEL| {fuel.Date} | {fuel.Gallons}G | ${fuel.Cost:F2} | {fuel.Mileage} mi");
        }

        foreach (var maint in data.MaintenanceLogs)
        {
            lines.Add($"MAINTENANCE| {maint.ServiceType} | {maint.Date} | {maint.Mileage} mi | {maint.IntervalMiles} mi.");
        }

        File.WriteAllLines(fileName, lines);
    }
}

class CarData
{
    public List<FuelLog> FuelLogs { get; set; } = new List<FuelLog>();
    public List<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
}

class FuelLog
{
    public string Date { get; set; }
    public double Gallons { get; set; }
    public double Cost { get; set; }
    public int Mileage { get; set; }
}

class MaintenanceLog
{
    public string ServiceType { get; set; }
    public string Date { get; set; }
    public int Mileage { get; set; }
    public int IntervalMiles { get; set; }
}