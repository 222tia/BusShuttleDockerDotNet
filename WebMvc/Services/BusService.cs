using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.DB;

namespace WebMvc.Services;

public class BusService : BusServiceInterface {

    private readonly BusShuttleDockerDB _busShuttleDockerDB;

    public BusService(BusShuttleDockerDB busShuttleDockerDB) {
        _busShuttleDockerDB = busShuttleDockerDB;
    }

    public List<BusModel> getAllBusses() {
        var allBusses = _busShuttleDockerDB.Bus.Select(b => new BusModel(b.Id, b.BusNumber)).ToList();
        return allBusses;
    }

    public void updateBusByID(int id, int newBusNumber) {
        var existingBus = _busShuttleDockerDB.Bus.Find(id);
        if (existingBus != null) {
            existingBus.BusNumber = newBusNumber;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createBus(int busNumber) {
        var newBus = new Database.Bus { 
            BusNumber = busNumber
        };
        _busShuttleDockerDB.Bus.Add(newBus);
        _busShuttleDockerDB.SaveChanges();
    }

    public BusModel? findBusByID(int id) {
        var bus = _busShuttleDockerDB.Bus.Find(id);
        if (bus != null) {
            return new BusModel(bus.Id, bus.BusNumber);
        }
        return null;
    }

    public void deleteBusByID(int id) {
        var existingBus = _busShuttleDockerDB.Bus.Find(id);
        if (existingBus != null) {
            _busShuttleDockerDB.Bus.Remove(bus);
            _busShuttleDockerDB.SaveChanges();
        }
    }
    
}
