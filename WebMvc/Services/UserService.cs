using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;

namespace WebMvc.Services;

public class UserService : UserServiceInterface {

    private readonly BusShuttleDocker _busShuttleDockerDB;

    public UserService(BusShuttleDocker busShuttleDockerDB) {
            _busShuttleDockerDB = busShuttleDockerDB;
    }

    public List<UserModel> getAllUsers() {
        var allUsers = _busShuttleDockerDB.User.Select(u => new UserModel(u.Id, u.FirstName, u.LastName, u.UserName, u.Password)).ToList();
        return allUsers;
    }

    public void updateUserByID(int id, string firstname, string lastname, string userName, string password) {
        var existingUser = _busShuttleDockerDB.User.Find(id);
        if (existingUser != null) {
            existingUser.FirstName = firstname;
            existingUser.LastName = lastname;
            existingUser.UserName = userName;
            existingUser.Password = password;
            _busShuttleDockerDB.SaveChanges();
        }
    }

    public void createUser(int id,string firstname, string lastname, string userName, string password) {
        var newUser = new Services.User { Id = id, FirstName = firstname, LastName = lastname, UserName = userName, Password = password };
        _busShuttleDockerDB.User.Add(newUser);
        _busShuttleDockerDB.SaveChanges();
    }

    public UserModel? findUserByID(int id) {
        var existingUser =  _busShuttleDockerDB.User.Find(id);
        if (existingUser != null) {
            return new UserModel(existingUser.Id, existingUser.FirstName, existingUser.LastName, existingUser.UserName, existingUser.Password);
        }
        return null;
    }

    public void deleteUserByID(int id) {
        var existingUser = _busShuttleDockerDB.User.Find(id);
        if (existingUser != null) {
            _busShuttleDockerDB.User.Remove(existingUser);
            _busShuttleDockerDB.SaveChanges();
        }
    }
        
}
