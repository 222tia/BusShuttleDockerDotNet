using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using DomainModel;

namespace WebMvc.Services;

public class RouteService : RouteServiceInterface {

    private readonly BusShuttleDocker _busShuttleDockerDB;

    public RouteService(BusShuttleDocker BusShuttleDocker) {
            _busShuttleDockerDB = BusShuttleDocker;
    }

    public List<RouteModel> getAllRoutes(){
        var allRoutes = _busShuttleDockerDB.Route.Select(r => new RouteModel(r.Id, r.Order)).ToList();
        return allRoutes;
    }

    public void updateRouteByID(int id, int order) {
        var existingRoute = _busShuttleDockerDB.Route.Find(id);
        if (existingRoute != null) {
            existingRoute.Order = order;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createRoute(int order) {
        var newRoute = new Services.Route { Order = order };
        _busShuttleDockerDB.Route.Add(newRoute);
        _busShuttleDockerDB.SaveChanges();
    }

    public RouteModel? findRouteByID(int id) {
        var existingRoute = _busShuttleDockerDB.Route.Find(id);
        if (existingRoute != null) {
            return new RouteModel(existingRoute.Id, existingRoute.Order);
        }
        return null;
    }
    
    public void deleteRouteByID(int id) {
        var existingRoute = _busShuttleDockerDB.Route.Find(id);
        if (existingRoute != null) {
            _busShuttleDockerDB.Route.Remove(existingRoute);
            _busShuttleDockerDB.SaveChanges();
        }
    }

}
