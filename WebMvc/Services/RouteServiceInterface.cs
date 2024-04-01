using DomainModel;

namespace WebMvc.Services;

public interface RouteServiceInterface {
    List<RouteModel> getAllRoutes();
    void updateRouteByID(int id, int order);
    void createRoute(int order);
    RouteModel? findRouteByID(int id);
    void deleteRouteByID(int id);
}