using DomainModel;

namespace WebMvc.Services;

public interface EntryServiceInterface {
    List<EntryModel> getAllEntries();
    void updateEntryByID(int id, int stopid, int loopid, int driverid, int busid,DateTime timeStamp, int boarded, int leftBehind);
    void createEntry( int stopid, int loopid, int driverid, int busid,DateTime timeStamp, int boarded, int leftBehind);
    EntryModel? findEntryByID(int id);
    void deleteEntryByID(int id);
    EntryModel? FindEntryByCredentials(int busid, int loopid, int driverid, int stopid);
}