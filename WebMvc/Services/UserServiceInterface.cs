using DomainModel;

namespace WebMvc.Services;

public interface UserServiceInterface {
    List<UserModel> getAllUsers();
    void updateUserByID(int id, string firstname, string lastname, string username, string password);
    void createUser(int id, string firstname, string lastname, string username, string password);
    UserModel? findUserByID(int id);
    void deleteUserByID(int id);
}