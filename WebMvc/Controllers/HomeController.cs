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
    DriverServiceInterface driverService;
    EntryServiceInterface entryService;
    LoopServiceInterface loopService;
    RouteServiceInterface routeService;
    StopServiceInterface stopService;
    UserServiceInterface userService;

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService , DriverServiceInterface driverService, EntryServiceInterface entryService, 
    LoopServiceInterface loopService, RouteServiceInterface routeService, StopServiceInterface stopService, UserServiceInterface userService) {
        _logger = logger;
        this.busService = busService;
        this.driverService = driverService;
        this.entryService = entryService;
        this.loopService = loopService;
        this.routeService = routeService;
        this.stopService = stopService;
        this.userService = userService;
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

    // ---------------- DRIVER ---------------- 

    public IActionResult DriverView() {
        return View(this.driverService.getAllDrivers().Select(d => DriverViewModel.FromDriver(d)));
    }

    public IActionResult DriverEdit([FromRoute] int id) {
        var driver = this.driverService.findDriverByID(id);
        return View(DriverEditModel.FromDriver(driver));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverEdit(int id, [Bind("FirstName, LastName")] DriverEditModel driver) {
        if (ModelState.IsValid) {
            this.driverService.updateDriverByID(id, driver.FirstName, driver.LastName);
            return RedirectToAction("DriverView");
        } else {
            return View(driver);
        }
    }

    public IActionResult DriverCreate() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverCreate([Bind("FirstName, LastName")] DriverCreateModel driver) {
        if (ModelState.IsValid) {
            this.driverService.createDriver(driver.FirstName, driver.LastName);
            return RedirectToAction("DriverView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DriverDelete(int id) {
        if (ModelState.IsValid) {
            this.driverService.deleteDriverByID(id);
            return RedirectToAction("DriverView");
        } else {
            return View();
        }
    }

    // ---------------- ENTRY ---------------- 

    public IActionResult EntryView() {
        return View(this.entryService.getAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }

    public IActionResult EntryEdit([FromRoute] int id) {
        var entry = this.entryService.findEntryByID(id);
        return View(EntryEditModel.FromEntry(entry));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EntryEdit(int id, [Bind("TimeStamp, Boarded, LeftBehind")] EntryEditModel entry) {
        if (ModelState.IsValid) {
            this.entryService.updateEntryByID(id, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            return RedirectToAction("EntryView");
        } else {
            return View(entry);
        }
    }

    public IActionResult EntryCreate() {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EntryCreate([Bind("TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry) {
        if (ModelState.IsValid) {
            this.entryService.createEntry(entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            return RedirectToAction("EntryView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EntryDelete(int id) {
        if (ModelState.IsValid) {
            this.entryService.deleteEntryByID(id);
            return RedirectToAction("EntryView");
        } else {
            return View();
        }
    }

    // ---------------- LOOP ---------------- 

    public IActionResult LoopView() {
        return View(this.loopService.getAllLoops().Select(l => LoopViewModel.FromLoop(l)));
    }

    public IActionResult LoopEdit([FromRoute] int id) {
        var loop = this.loopService.findLoopByID(id);
        return View(LoopEditModel.FromLoop(loop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoopEdit(int id, [Bind("Name")] LoopEditModel loop) {
        if (ModelState.IsValid) {
            this.loopService.updateLoopByID(id, loop.Name);
            return RedirectToAction("LoopView");
        } else {
            return View(loop);
        }
    }

    public IActionResult LoopCreate(){
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoopCreate([Bind("Name")] LoopCreateModel loop) {
        if (ModelState.IsValid) {
            this.loopService.createLoop(loop.Name);
            return RedirectToAction("LoopView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LoopDelete(int id) {
        if (ModelState.IsValid) {
            this.loopService.deleteLoopByID(id);
            return RedirectToAction("LoopView");
        } else {
            return View();
        }
    }

    // ---------------- ROUTE ---------------- 

    public IActionResult RouteView() {
        return View(this.routeService.getAllRoutes().Select(r => RouteViewModel.FromRoute(r)));
    }

    public IActionResult RouteEdit([FromRoute] int id) {
        var route = this.routeService.findRouteByID(id);
        return View(RouteEditModel.FromRoute(route));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RouteEdit(int id, [Bind("Order")] RouteEditModel route) {
        if (ModelState.IsValid) {
            this.routeService.updateRouteByID(id, route.Order);
            return RedirectToAction("RouteView");
        } else {
            return View(route);
        }
    }

    public IActionResult RouteCreate() {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RouteCreate([Bind("Order")] RouteCreateModel route) {
        if (ModelState.IsValid) {
            this.routeService.createRoute(route.Order);
            return RedirectToAction("RouteView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RouteDelete(int id) {
        if (ModelState.IsValid) {
            this.routeService.deleteRouteByID(id);
            return RedirectToAction("RouteView");
        } else {
            return View();
        }
    }

    // ---------------- STOP ---------------- 

    public IActionResult StopView() {
        return View(this.stopService.getAllStops().Select(s => StopViewModel.FromStop(s)));
    }

    public IActionResult StopEdit([FromRoute] int id){
        var stop = this.stopService.findStopByID(id);
        return View(StopEditModel.FromStop(stop));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StopEdit(int id, [Bind("Name, Latitude, Longitude")] StopEditModel stop) {
        if (ModelState.IsValid) {
            this.stopService.updateStopByID(id, stop.Name, stop.Latitude, stop.Longitude);
            return RedirectToAction("StopView");
        } else {
            return View(stop);
        }
    }

    public IActionResult StopCreate() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StopCreate([Bind("Name, Latitude, Longitude")] StopCreateModel stop) {
        if (ModelState.IsValid) {
            this.stopService.createStop(stop.Name, stop.Latitude, stop.Longitude);
            return RedirectToAction("StopView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StopDelete(int id) {
        if (ModelState.IsValid) {
            this.stopService.deleteStopByID(id);
            return RedirectToAction("StopView");
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
