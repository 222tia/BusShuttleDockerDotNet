using DomainModel;

namespace WebMvc.Services;

public interface LoopServiceInterface {
    List<LoopModel> getAllLoops();
    void updateLoopByID(int id, string name);
    void createLoop(string name);
    LoopModel? findLoopByID(int id);
    void deleteLoopByID(int id);
}