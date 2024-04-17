using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMvc.Models;
using WebMvc.Services;
using DomainModel;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BusShuttleDocker _database;
    BusServiceInterface busService;
    DriverServiceInterface driverService;
    EntryServiceInterface entryService;
    LoopServiceInterface loopService;
    RouteServiceInterface routeService;
    StopServiceInterface stopService;
    UserServiceInterface userService;

    public HomeController(ILogger<HomeController> logger, BusServiceInterface busService , DriverServiceInterface driverService, EntryServiceInterface entryService, 
    LoopServiceInterface loopService, RouteServiceInterface routeService, StopServiceInterface stopService, UserServiceInterface userService, BusShuttleDocker database) {
        _logger = logger;
        _database = database;
        this.busService = busService;
        this.driverService = driverService;
        this.entryService = entryService;
        this.loopService = loopService;
        this.routeService = routeService;
        this.stopService = stopService;
        this.userService = userService;
    }

    public IActionResult ManagerDashboard() {
        return View();
    }

    public IActionResult DriverDashboard() {
        ViewBag.Loops = this.loopService.getAllLoops().Select(l => new SelectListItem { 
            Value = l.Id.ToString(), Text = l.Name}).ToList();

        ViewBag.Buses = this.busService.getAllBusses().Select(b => new SelectListItem { 
            Value = b.Id.ToString(), Text = b.BusNumber.ToString()}).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DriverDashboard([Bind("StopId, TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry)
    {
        return RedirectToAction("DriverEntryCreate");
    }

    public IActionResult DriverEntryCreate() {
        ViewBag.Stops = this.stopService.getAllStops().Select(s => new SelectListItem { 
            Value = s.Id.ToString(), Text = s.Name}).ToList();
            
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverEntryCreate([Bind("TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry) {
        if (ModelState.IsValid) {
            this.entryService.createEntry(entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            await _database.SaveChangesAsync();
            return RedirectToAction("DriverEntryCreate");
        } else {
            return View();
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index([Bind("UserName, Password")] UserModel user)
    {
        if (ModelState.IsValid)
        {
            if (this.userService.isManager(user.UserName, user.Password))
            {
                return RedirectToAction("ManagerDashboard");
            }
            else if (this.userService.isDriver(user.UserName, user.Password))
            {
                return RedirectToAction("DriverDashboard");
            }
            else
            {
                return View();
            }
        }
        else
        {
            return View();
        }
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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
            await _database.SaveChangesAsync();
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

    public IActionResult UserView() {
        return View(this.userService.getAllUsers().Select(u => UserViewModel.FromUser(u)));
    }

    public IActionResult UserEdit([FromRoute] int id) {
        var user = this.userService.findUserByID(id);
        return View(UserEditModel.FromUser(user));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserEdit(int id, [Bind("FirstName, LastName, UserName, Password")] UserEditModel user) {
        if (ModelState.IsValid) {
            this.userService.updateUserByID(id, user.FirstName, user.LastName, user.UserName, user.Password);
            await _database.SaveChangesAsync();
            return RedirectToAction("UserView");
        } else {
            return View(user);
        }
    }

    public IActionResult UserCreate() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserCreate([Bind("FirstName, LastName, UserName, Password")] UserEditModel user) {
        if (ModelState.IsValid) {
            this.userService.createUser(user.FirstName, user.LastName, user.UserName, user.Password);
            await _database.SaveChangesAsync();
            return RedirectToAction("UserView");
        } else {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UserDelete(int id) {
        if (ModelState.IsValid) {
            this.userService.deleteUserByID(id);
            return RedirectToAction("UserView");
        } else {
            return View();
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
