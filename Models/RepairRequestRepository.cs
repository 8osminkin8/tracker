using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Models;

public class RepairRequestRepository
{
    private readonly AppDbContext _context;

    public RepairRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<RepairRequest> GetAll()
    {
        return _context.RepairRequests.ToList();
    }

    public RepairRequest GetById(int id)
    {
        return _context.RepairRequests.Find(id);
    }

    public IEnumerable<RepairRequest> SearchByType(RepairType type)
    {
        return _context.RepairRequests
                       .Where(r => r.Type == type)
                       .ToList();
    }

    public void Add(RepairRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        _context.RepairRequests.Add(request);
        _context.SaveChanges();
    }

    public void Update(RepairRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        _context.RepairRequests.Update(request);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var request = _context.RepairRequests.Find(id);
        if (request != null)
        {
            _context.RepairRequests.Remove(request);
            _context.SaveChanges();
        }
    }
    public Dictionary<DateTime, Dictionary<RepairType, int>> GetRequestCountsGroupedByDate()
    {
        return _context.RepairRequests
            .ToList() // Assuming the dataset is not too large and can be brought into memory
            .GroupBy(request => request.RequestDate.Date)
            .ToDictionary(
                groupByDate => groupByDate.Key,
                groupByDate => groupByDate
                    .GroupBy(request => request.Type)
                    .ToDictionary(
                        groupByType => groupByType.Key,
                        groupByType => groupByType.Count()
                    )
            );
    }
}
