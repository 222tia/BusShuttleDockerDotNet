using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;

namespace WebMvc.Services;

public class LoopService : LoopServiceInterface {

    private readonly BusShuttleDocker _busShuttleDockerDB;

    public LoopService(BusShuttleDocker busShuttleDockerDB) {
        _busShuttleDockerDB = busShuttleDockerDB;
    }

    public List<LoopModel> getAllLoops() {
        var allLoops = _busShuttleDockerDB.Loop.Select(l => new LoopModel(l.Id, l.Name)).ToList();
        return allLoops;
    }

    public void updateLoopByID(int id, string name) {
        var existingLoop = _busShuttleDockerDB.Loop.Find(id);
        if (existingLoop != null) {
            existingLoop.Name = name;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createLoop(string name) {
        var newLoop = new Services.Loop { Name = name };
        _busShuttleDockerDB.Loop.Add(newLoop);
        _busShuttleDockerDB.SaveChanges();
    }

    public LoopModel? findLoopByID(int id) {
        var existingLoop = _busShuttleDockerDB.Loop.Find(id);
        if (existingLoop != null) {
            return new LoopModel(existingLoop.Id, existingLoop.Name);
        }
        return null;
    }

    public void deleteLoopByID(int id) {
        var existingLoop = _busShuttleDockerDB.Loop.Find(id);
        if (existingLoop != null) {
            _busShuttleDockerDB.Loop.Remove(existingLoop);
            _busShuttleDockerDB.SaveChanges();
        }
    }
    
}
