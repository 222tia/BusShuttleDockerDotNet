using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;

namespace WebMvc.Services;

public class StopService : StopServiceInterface {
        
    private readonly BusShuttleDocker _busShuttleDockerDB;

    public StopService(BusShuttleDocker busShuttleDockerDB) {
        _busShuttleDockerDB = busShuttleDockerDB;
    }

    public List<StopModel> getAllStops() {
        var allStops = _busShuttleDockerDB.Stop.Select(s => new StopModel(s.Id, s.Name, s.Latitude, s.Longitude)).ToList();
        return allStops;
    }

    public void updateStopByID(int id, string name, double latitude, double longitude) {
        var existingStop = _busShuttleDockerDB.Stop.Find(id);
        if (existingStop != null) {
            existingStop.Name = name;
            existingStop.Latitude = latitude;
            existingStop.Longitude = longitude;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createStop(string name, double latitude, double longitude) {
        var newStop = new Services.Stop { Name = name, Latitude = latitude, Longitude = longitude };
        _busShuttleDockerDB.Stop.Add(newStop);
        _busShuttleDockerDB.SaveChanges();
    }

    public StopModel? findStopByID(int id) {
        var existingStop = _busShuttleDockerDB.Stop.Find(id);
        if (existingStop != null) {
            return new StopModel(existingStop.Id, existingStop.Name, existingStop.Latitude, existingStop.Longitude);
        }
        return null;
    }

    public void deleteStopByID(int id) {
        var existingStop = _busShuttleDockerDB.Stop.Find(id);
        if (existingStop != null) {
            _busShuttleDockerDB.Stop.Remove(existingStop);
            _busShuttleDockerDB.SaveChanges();
        }
    }

}
