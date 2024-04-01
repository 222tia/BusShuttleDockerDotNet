using DomainModel;

namespace WebMvc.Services;

public interface BusServiceInterface {
    List<BusModel> getAllBusses();
    void updateBusByID(int id, int busNumber);
    void createBus(int id, int busNumber);
    BusModel? findBusByID(int id);
    void deleteBusByID(int id);
}