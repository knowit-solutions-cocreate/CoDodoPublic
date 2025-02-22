﻿using System.Text;

namespace CoDodoApi.Entities;

public sealed class Process
{
    public string Name { get; set; } = "";
    public Opportunity Opportunity { get; set; }
    public string Status { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public TimeProvider TimeProvider { get; set; }

    public Process(string name,
                   Opportunity opportunity,
                   string status,
                   DateTimeOffset createdDate,
                   DateTimeOffset updatedDate,
                   TimeProvider provider)
    {
        Name = name;
        Opportunity = opportunity;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        TimeProvider = provider;
    }

    public int DaysSinceUpdate()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - UpdatedDate;

        return NumberOfWholeDays(diff);
    }

    public int DaysSinceCreation()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - CreatedDate;

        return NumberOfWholeDays(diff);
    }

    internal string Key()
    {
        string t = Name + Opportunity.UriForAssignment;

        byte[] b = Encoding.UTF8.GetBytes(t);

        return Convert.ToBase64String(b);
    }

    static int NumberOfWholeDays(TimeSpan diff)
    {
        double numberOfDays = diff.TotalDays;

        return (int)numberOfDays;
    }

    internal bool IsWon()
    {
        return Status == "WON";
    }
}

internal static class ProcessExtensions
{
    public static ProcessDTO ToDto(this Process process)
    {
        Process p = process;
        Opportunity d = p.Opportunity;

        return new ProcessDTO(p.Name,
                              d.UriForAssignment,
                              d.Company,
                              d.Capability,
                              p.Status,
                              d.NameOfSalesLead,
                              d.HourlyRateInSEK,
                              p.UpdatedDate,
                              p.CreatedDate,
                              p.DaysSinceUpdate(),
                              p.DaysSinceCreation());
    }
}