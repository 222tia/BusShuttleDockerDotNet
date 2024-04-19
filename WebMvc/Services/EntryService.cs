using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;

namespace WebMvc.Services;

    public class EntryService : EntryServiceInterface {
    
    private readonly BusShuttleDocker _busShuttleDockerDB;

    public EntryService(BusShuttleDocker busShuttleDockerDB) {
        _busShuttleDockerDB = busShuttleDockerDB;
    }
    
    public List<EntryModel> getAllEntries(){
        var allEntries = _busShuttleDockerDB.Entry.Select(e => new EntryModel(e.Id, e.StopId, e.LoopId, e.DriverId, e.TimeStamp, e.Boarded, e.LeftBehind)).ToList();
        return allEntries;
    }

    public void updateEntryByID(int id, int stopid, int loopid, int driverid, DateTime timeStamp, int boarded, int leftBehind) {
        var existingEntry = _busShuttleDockerDB.Entry.Find(id);
        if (existingEntry != null) {
            existingEntry.StopId = stopid;
            existingEntry.LoopId = loopid;
            existingEntry.DriverId = driverid;
            existingEntry.TimeStamp = timeStamp;
            existingEntry.Boarded = boarded;
            existingEntry.LeftBehind = leftBehind;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createEntry(int stopid, int loopid, int driverid, DateTime timeStamp, int boarded, int leftBehind) {
        var newEntry = new Services.Entry {
            StopId = stopid,
            LoopId = loopid,
            DriverId = driverid,
            TimeStamp = timeStamp, 
            Boarded = boarded, 
            LeftBehind = leftBehind 
            };
        _busShuttleDockerDB.Entry.Add(newEntry);
        _busShuttleDockerDB.SaveChanges();
    }

    public EntryModel? findEntryByID(int id) {
        var existingEntry = _busShuttleDockerDB.Entry.Find(id);
        if (existingEntry != null){
            return new EntryModel(existingEntry.Id, existingEntry.TimeStamp, existingEntry.Boarded, existingEntry.LeftBehind);
        }
        return null;
    }

    public void deleteEntryByID(int id) {
        var existingEntry = _busShuttleDockerDB.Entry.Find(id);
        if (existingEntry != null) {
            _busShuttleDockerDB.Entry.Remove(existingEntry);
            _busShuttleDockerDB.SaveChanges();
        }
    }

}
