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

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index([Bind("UserName, Password")] UserModel user) {
        if (ModelState.IsValid) {
            if (this.userService.isManager(user.UserName, user.Password)) {
                _logger.LogInformation("Passed manager authentication - username:" + user.UserName.ToString() + " password:" + user.Password.ToString());
                return RedirectToAction("ManagerDashboard");
            } else if (this.userService.isDriver(user.UserName, user.Password)) {
                _logger.LogInformation("Passed driver authentication - username:" + user.UserName.ToString() + " password:" + user.Password.ToString());
                return RedirectToAction("DriverDashboard");
            } else {
                _logger.LogError("Failed user authentication");
                return View();
            }
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }


    public IActionResult Privacy() {
        return View();
    }

    // ---------------- REPORT ---------------- 

    public IActionResult FilterReports() {
        ViewBag.Loops = this.loopService.getAllLoops().Select(l => new SelectListItem { 
            Value = l.Id.ToString(), Text = l.Name}).ToList();

        ViewBag.Buses = this.busService.getAllBusses().Select(b => new SelectListItem { 
            Value = b.Id.ToString(), Text = b.BusNumber.ToString()}).ToList();

        ViewBag.Drivers = this.driverService.getAllDrivers().Select(d => new SelectListItem { 
            Value = d.Id.ToString(), Text = d.FirstName.ToString() + " " + d.LastName.ToString() }).ToList();

        ViewBag.Stops = this.stopService.getAllStops().Select(s => new SelectListItem { 
            Value = s.Id.ToString(), Text = s.Name}).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ViewReports([FromRoute]int busid, int loopid, int driverid, int stopid) {
        var entry = this.entryService.FindEntryByCredentials(busid, loopid, driverid, stopid);
        if (entry != null) {
            return View(EntryViewModel.FromEntry(entry));
        } else {
            return View();
        }
    }

    // ---------------- DRIVER LOGIN ---------------- 

    public IActionResult DriverDashboard() {
        ViewBag.Loops = this.loopService.getAllLoops().Select(l => new SelectListItem { 
            Value = l.Id.ToString(), Text = l.Name}).ToList();

        ViewBag.Buses = this.busService.getAllBusses().Select(b => new SelectListItem { 
            Value = b.Id.ToString(), Text = b.BusNumber.ToString()}).ToList();

        ViewBag.Drivers = this.driverService.getAllDrivers().Select(d => new SelectListItem { 
            Value = d.Id.ToString(), Text = d.FirstName.ToString() + " " + d.LastName.ToString() }).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DriverDashboard(EntryCreateModel entry) {
        if (ModelState.IsValid) {
            return RedirectToAction("DriverEntryCreate");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
        
    }

    public IActionResult DriverEntryCreate() {
        ViewBag.Stops = this.stopService.getAllStops().Select(s => new SelectListItem { 
            Value = s.Id.ToString(), Text = s.Name}).ToList();
            
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverEntryCreate([Bind("Stop, Loop, Driver, Bus TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry) {
        if (ModelState.IsValid) {
            this.entryService.createEntry(entry.StopId, entry.LoopId, entry.DriverId, entry.BusId, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            await _database.SaveChangesAsync();
            _logger.LogInformation("New entry added - stop:" + entry.StopId.ToString() + 
            " loop:" + entry.LoopId.ToString() +
            " driver:" + entry.DriverId.ToString() +
            " bus:" + entry.BusId.ToString() +
            " timestamp:" + entry.TimeStamp.ToString() +
            " boarded:" + entry.Boarded.ToString() +
            " leftbehind:" + entry.LeftBehind.ToString()
            );
            return RedirectToAction("DriverEntryCreate");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
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
            _logger.LogInformation("New bus added - id:" + bus.Id.ToString() + " busNumber:" + bus.BusNumber.ToString());
            return RedirectToAction("BusView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    public IActionResult BusEdit([FromRoute] int id) {
        var bus = this.busService.findBusByID(id);
        if (bus != null) {
            return View(BusEditModel.FromBus(bus));
        } else {
            _logger.LogError("Bus with id " + id.ToString() + " could not be found");
            return View(bus);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusEdit(int id, [Bind("BusNumber")] BusEditModel bus) {
        if (ModelState.IsValid) {
            this.busService.updateBusByID(id, bus.BusNumber);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Bus edited - busNumber:" + bus.BusNumber.ToString());
            return RedirectToAction("BusView");
        } else {
            _logger.LogError("Model state invalid");
            return View(bus);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BusDelete(int id) {
        if (ModelState.IsValid) {
            this.busService.deleteBusByID(id);
            _logger.LogInformation("Bus deleted");
            return RedirectToAction("BusView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- DRIVER ---------------- 

    public IActionResult DriverView() {
        return View(this.driverService.getAllDrivers().Select(d => DriverViewModel.FromDriver(d)));
    }

    public IActionResult DriverEdit([FromRoute] int id) {
        var driver = this.driverService.findDriverByID(id);
        if (driver != null) {
            return View(DriverEditModel.FromDriver(driver));
        } else {
            _logger.LogError("Driver with id " + id.ToString() + " could not be found");
            return View(driver);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DriverEdit(int id, [Bind("FirstName, LastName")] DriverEditModel driver) {
        if (ModelState.IsValid) {
            this.driverService.updateDriverByID(id, driver.FirstName, driver.LastName);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Driver edited - FirstName:" + driver.FirstName.ToString() + " LastName: " + driver.LastName.ToString());
            return RedirectToAction("DriverView");
        } else {
            _logger.LogError("Model state invalid");
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
            _logger.LogInformation("Driver added - FirstName:" + driver.FirstName.ToString() + " LastName: " + driver.LastName.ToString());
            return RedirectToAction("DriverView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DriverDelete(int id) {
        if (ModelState.IsValid) {
            this.driverService.deleteDriverByID(id);
            _logger.LogInformation("Driver deleted");
            return RedirectToAction("DriverView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- ENTRY ---------------- 

    public IActionResult EntryView() {
        return View(this.entryService.getAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }

    public IActionResult EntryEdit([FromRoute] int id) {
        var entry = this.entryService.findEntryByID(id);
        if (entry != null) {
            return View(EntryEditModel.FromEntry(entry));
        } else {
            _logger.LogError("Entry with id " + id.ToString() + " could not be found");
            return View(entry);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EntryEdit(int id, [Bind("Stop, Loop, Driver, Bus TimeStamp, Boarded, LeftBehind")] EntryEditModel entry) {
        if (ModelState.IsValid) {
            this.entryService.updateEntryByID(id, entry.StopId, entry.LoopId, entry.DriverId, entry.BusId, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Entry edited - stop:" + entry.StopId.ToString() + 
            " loop:" + entry.LoopId.ToString() +
            " driver:" + entry.DriverId.ToString() +
            " bus:" + entry.BusId.ToString() +
            " timestamp:" + entry.TimeStamp.ToString() +
            " boarded:" + entry.Boarded.ToString() +
            " leftbehind:" + entry.LeftBehind.ToString());
            return RedirectToAction("EntryView");
        } else {
            _logger.LogError("Model state invalid");
            return View(entry);
        }
    }

    public IActionResult EntryCreate() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EntryCreate([Bind("Stop, Loop, Driver, Bus, TimeStamp, Boarded, LeftBehind")] EntryCreateModel entry) {
        if (ModelState.IsValid) {
            this.entryService.createEntry(entry.StopId, entry.LoopId, entry.DriverId, entry.BusId, entry.TimeStamp, entry.Boarded, entry.LeftBehind);
            await _database.SaveChangesAsync();
            _logger.LogInformation("New entry added - stop:" + entry.StopId.ToString() + 
            " loop:" + entry.LoopId.ToString() +
            " driver:" + entry.DriverId.ToString() +
            " bus:" + entry.BusId.ToString() +
            " timestamp:" + entry.TimeStamp.ToString() +
            " boarded:" + entry.Boarded.ToString() +
            " leftbehind:" + entry.LeftBehind.ToString());
            return RedirectToAction("EntryView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EntryDelete(int id) {
        if (ModelState.IsValid) {
            this.entryService.deleteEntryByID(id);
            _logger.LogInformation("Entry deleted");
            return RedirectToAction("EntryView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- LOOP ---------------- 

    public IActionResult LoopView() {
        return View(this.loopService.getAllLoops().Select(l => LoopViewModel.FromLoop(l)));
    }

    public IActionResult LoopEdit([FromRoute] int id) {
        var loop = this.loopService.findLoopByID(id);
        if (loop != null) {
            return View(LoopEditModel.FromLoop(loop));
        } else {
            _logger.LogError("Loop with id " + id.ToString() + " could not be found");
            return View(loop);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoopEdit(int id, [Bind("Name")] LoopEditModel loop) {
        if (ModelState.IsValid) {
            this.loopService.updateLoopByID(id, loop.Name);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Loop edited - Name:" + loop.Name.ToString());
            return RedirectToAction("LoopView");
        } else {
            _logger.LogError("Model state invalid");
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
            _logger.LogInformation("New loop added - Name:" + loop.Name.ToString());
            return RedirectToAction("LoopView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LoopDelete(int id) {
        if (ModelState.IsValid) {
            this.loopService.deleteLoopByID(id);
            _logger.LogInformation("Loop deleted");
            return RedirectToAction("LoopView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- ROUTE ---------------- 

    public IActionResult RouteView() {
        return View(this.routeService.getAllRoutes().Select(r => RouteViewModel.FromRoute(r)));
    }

    public IActionResult RouteEdit([FromRoute] int id) {
        var route = this.routeService.findRouteByID(id);
        if (route != null) {
            return View(RouteEditModel.FromRoute(route));
        } else {
            _logger.LogError("Route with id " + id.ToString() + " could not be found");
            return View(route);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RouteEdit(int id, [Bind("Order")] RouteEditModel route) {
        if (ModelState.IsValid) {
            this.routeService.updateRouteByID(id, route.Order);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Route edited - Order:" + route.Order.ToString());
            return RedirectToAction("RouteView");
        } else {
            _logger.LogError("Model state invalid");
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
            _logger.LogInformation("New route added - Order:" + route.Order.ToString());
            return RedirectToAction("RouteView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RouteDelete(int id) {
        if (ModelState.IsValid) {
            this.routeService.deleteRouteByID(id);
            _logger.LogInformation("Route deleted");
            return RedirectToAction("RouteView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- STOP ---------------- 

    public IActionResult StopView() {
        return View(this.stopService.getAllStops().Select(s => StopViewModel.FromStop(s)));
    }

    public IActionResult StopEdit([FromRoute] int id){
        var stop = this.stopService.findStopByID(id);
        if (stop != null) {
            return View(StopEditModel.FromStop(stop));
        } else {
            _logger.LogError("Stop with id " + id.ToString() + " could not be found");
            return View(stop);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StopEdit(int id, [Bind("Name, Latitude, Longitude")] StopEditModel stop) {
        if (ModelState.IsValid) {
            this.stopService.updateStopByID(id, stop.Name, stop.Latitude, stop.Longitude);
            await _database.SaveChangesAsync();
            _logger.LogInformation("Stop edited - Name:" + stop.Name.ToString() + 
            " Latitude: " + stop.Latitude.ToString() +
            " Longitude: " + stop.Longitude.ToString());
            return RedirectToAction("StopView");
        } else {
            _logger.LogError("Model state invalid");
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
            _logger.LogInformation("New stop added - Name:" + stop.Name.ToString() + 
            " Latitude: " + stop.Latitude.ToString() +
            " Longitude: " + stop.Longitude.ToString());
            return RedirectToAction("StopView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StopDelete(int id) {
        if (ModelState.IsValid) {
            this.stopService.deleteStopByID(id);
            _logger.LogInformation("Stop deleted");
            return RedirectToAction("StopView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    // ---------------- USER ---------------- 

    public IActionResult UserView() {
        return View(this.userService.getAllUsers().Select(u => UserViewModel.FromUser(u)));
    }

    public IActionResult UserEdit([FromRoute] int id) {
        var user = this.userService.findUserByID(id);
        if (user != null) {
            return View(UserEditModel.FromUser(user));
        } else {
            _logger.LogError("User with id " + id.ToString() + " could not be found");
            return View(user);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserEdit(int id, [Bind("FirstName, LastName, UserName, Password")] UserEditModel user) {
        if (ModelState.IsValid) {
            this.userService.updateUserByID(id, user.FirstName, user.LastName, user.UserName, user.Password);
            await _database.SaveChangesAsync();
            _logger.LogInformation("User edited - FirstName:" + user.FirstName.ToString() + 
            " LastName: " + user.LastName.ToString() +
            " UserName: " + user.UserName.ToString() +
            " Password: " + user.Password.ToString());
            return RedirectToAction("UserView");
        } else {
            _logger.LogError("Model state invalid");
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
            _logger.LogInformation("New user added - FirstName:" + user.FirstName.ToString() + 
            " LastName: " + user.LastName.ToString() +
            " UserName: " + user.UserName.ToString() +
            " Password: " + user.Password.ToString());
            return RedirectToAction("UserView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UserDelete(int id) {
        if (ModelState.IsValid) {
            this.userService.deleteUserByID(id);
            _logger.LogInformation("User deleted");
            return RedirectToAction("UserView");
        } else {
            _logger.LogError("Model state invalid");
            return View();
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
