using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.DB;

namespace WebMvc.Services;

public class DriverService : DriverServiceInterface {

    private readonly BusShuttleDocker _busShuttleDockerDB;

    public DriverService(BusShuttleDocker busShuttleDockerDB) {
            _busShuttleDockerDB = busShuttleDockerDB;
    }

    public List<DriverModel> getAllDrivers() {
        var allDrivers = _busShuttleDockerDB.Driver.Select(d => new DriverModel(d.Id, d.FirstName, d.LastName)).ToList();
        return allDrivers;
    }

    public void updateDriverByID(int id, string firstname, string lastname) {
        var existingDriver = _busShuttleDockerDB.Driver.Find(id);
        if (existingDriver != null) {
            existingDriver.FirstName = firstname;
            existingDriver.LastName = lastname;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createDriver(string firstname, string lastname) {
        var newDriver = new DB.Driver { FirstName = firstname, LastName = lastname };
        _busShuttleDockerDB.Driver.Add(newDriver);
        _busShuttleDockerDB.SaveChanges();
    }

    public DriverModel? findDriverByID(int id) {
        var existingDriver = _busShuttleDockerDB.Driver.Find(id);
        if (existingDriver != null) {
            return new DriverModel(existingDriver.Id, existingDriver.FirstName, existingDriver.LastName);
        }
        return null;
    }

    public void deleteDriverByID(int id) {
        var existingDriver = _busShuttleDockerDB.Driver.Find(id);
        if (existingDriver != null){
            _busShuttleDockerDB.Driver.Remove(existingDriver);
            _busShuttleDockerDB.SaveChanges();
        }
    }

}
