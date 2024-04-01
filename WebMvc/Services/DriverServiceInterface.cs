using DomainModel;
namespace WebMvc.Services;

public interface DriverServiceInterface
{
    List<DriverModel> getAllDrivers();
    void updateDriverByID(int id, string firstname, string lastname);
    void createDriver(string firstname, string lastname);
    DriverModel? findDriverByID(int id);
    void deleteDriverByID(int id);
}