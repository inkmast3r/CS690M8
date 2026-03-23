namespace CarCareTracker;
using System;
using System.Collections.Generic;
using System.IO;

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
            string trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            // skip headers and separators
            if (trimmed.StartsWith("*") || trimmed.StartsWith("-"))
                continue;

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
