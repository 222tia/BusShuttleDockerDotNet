using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Services;
using DomainModel;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    BusServiceInterface busService;

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService)
    {
        _logger = logger;
        this.busService = busService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

     // ---------------- BUS ---------------- 

    public IActionResult BusView() {
        return View(this.busService.getAllBusses().Select(b => BusViewModel.FromBus(b)));
    }

    public IActionResult BusCreate(){
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusCreate([Bind("BusNumber")] BusCreateModel bus) {
        if (ModelState.IsValid) {
            this.busService.createBus(bus.Id, bus.BusNumber);
            return RedirectToAction("BusView");
        } else {
            return View();
        }
    }

    public IActionResult BusEdit([FromRoute] int id) {
        var bus = this.busService.findBusByID(id);
        return View(BusEditModel.FromBus(bus));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusEdit(int id, [Bind("BusNumber")] BusEditModel bus) {
        if (ModelState.IsValid) {
            this.busService.updateBusByID(id, bus.BusNumber);
            return RedirectToAction("BusView");
        } else {
            return View(bus);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BusDelete(int id) {
        if (ModelState.IsValid) {
            this.busService.deleteBusByID(id);
            return RedirectToAction("BusView");
        } else {
            return View();
        }
    }

    // ---------------- USER ---------------- 


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
