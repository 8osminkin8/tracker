using Microsoft.AspNetCore.Mvc;
using Tracker.Models; // Замените на ваше реальное пространство имен
using System;
using System.Linq;

public class RepairRequestsController : Controller
{
    private readonly RepairRequestService _repairRequestService;

    public RepairRequestsController(RepairRequestService repairRequestService)
    {
        _repairRequestService = repairRequestService;
    }

    // GET: RepairRequests
    public IActionResult Index(string searchType = "")
    {
        IEnumerable<RepairRequest> repairRequests;
        if (string.IsNullOrEmpty(searchType))
        {
            repairRequests = _repairRequestService.GetAllRepairRequests();
        }
        else
        {
            if (Enum.TryParse(searchType, out RepairType type))
            {
                repairRequests = _repairRequestService.GetRepairRequestsByType(type);
            }
            else
            {
                // Handle the case where the searchType is invalid
                repairRequests = Enumerable.Empty<RepairRequest>();
                ModelState.AddModelError(string.Empty, "Invalid type of repair.");
            }
        }

        // Prepare the chart data
        var chartData = _repairRequestService.GetChartData();
        ViewBag.Dates = chartData.Dates;
        ViewBag.CountsByType = chartData.CountsByType;

        return View(repairRequests);
    }

    // GET: RepairRequests/Create
    public IActionResult Create()
    {
        ViewBag.RepairTypes = Enum.GetValues(typeof(RepairType)).Cast<RepairType>();
        return View();
    }

    // POST: RepairRequests/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,Type,RequestDate")] RepairRequest repairRequest)
    {
        if (ModelState.IsValid)
        {
            _repairRequestService.CreateRepairRequest(repairRequest.Title, repairRequest.Type, repairRequest.RequestDate);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RepairTypes = Enum.GetValues(typeof(RepairType)).Cast<RepairType>();
        return View(repairRequest);
    }
}
