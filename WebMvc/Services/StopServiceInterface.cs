using DomainModel;

namespace WebMvc.Services;

public interface StopServiceInterface
{
    List<StopModel> getAllStops();
    void updateStopByID(int id, string name, double latitude, double longitude);
    void createStop(string name, double latitude, double longitude);
    StopModel? findStopByID(int id);
    void deleteStopByID(int id);
}