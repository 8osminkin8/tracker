using Tracker.Models; // Замените на ваше реальное пространство имен
using System;
using System.Collections.Generic;

public class RepairRequestService
{
    private readonly RepairRequestRepository _repairRequestRepository;

    public RepairRequestService(RepairRequestRepository repairRequestRepository)
    {
        _repairRequestRepository = repairRequestRepository ?? throw new ArgumentNullException(nameof(repairRequestRepository));
    }

    public IEnumerable<RepairRequest> GetAllRepairRequests()
    {
        return _repairRequestRepository.GetAll();
    }

    public RepairRequest GetRepairRequestById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid ID", nameof(id));
        }

        return _repairRequestRepository.GetById(id);
    }

    public IEnumerable<RepairRequest> GetRepairRequestsByType(RepairType type)
    {
        return _repairRequestRepository.SearchByType(type);
    }

    public void CreateRepairRequest(string title, RepairType type, DateTime requestDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required", nameof(title));
        }

        var newRequest = new RepairRequest
        {
            Title = title,
            Type = type,
            RequestDate = requestDate
        };

        _repairRequestRepository.Add(newRequest);
    }

    public void UpdateRepairRequest(RepairRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        _repairRequestRepository.Update(request);
    }

    public void DeleteRepairRequest(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid ID", nameof(id));
        }

        _repairRequestRepository.Delete(id);
    }
    public ChartDataDTO GetChartData()
    {
        // Get the raw data from the repository
        var rawData = _repairRequestRepository.GetRequestCountsGroupedByDate();

        // Prepare the DTO
        var chartDataDto = new ChartDataDTO
        {
            Dates = rawData.Keys.Select(date => date.ToShortDateString()).ToArray(),
            CountsByType = new Dictionary<string, int[]>()
        };

        // Initialize the dictionary for each repair type
        foreach (RepairType type in Enum.GetValues(typeof(RepairType)))
        {
            chartDataDto.CountsByType[type.ToString()] = new int[chartDataDto.Dates.Length];
        }

        // Fill the counts for each type and date
        int dateIndex = 0;
        foreach (var dateGroup in rawData)
        {
            foreach (RepairType type in Enum.GetValues(typeof(RepairType)))
            {
                chartDataDto.CountsByType[type.ToString()][dateIndex] =
                    dateGroup.Value.TryGetValue(type, out var count) ? count : 0;
            }
            dateIndex++;
        }

        return chartDataDto;
    }
}
