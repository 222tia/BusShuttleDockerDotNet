using DomainModel;

namespace WebMvc.Services;

public interface UserServiceInterface {
    List<UserModel> getAllUsers();
    void updateUserByID(int id, string firstname, string lastname, string username, string password);
    void createUser(string firstname, string lastname, string username, string password);
    UserModel? findUserByID(int id);
    void deleteUserByID(int id);
    bool isManager(string userName, string password);
}