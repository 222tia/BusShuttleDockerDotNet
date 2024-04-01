using DomainModel;

namespace WebMvc.Services;

public interface EntryServiceInterface {
    List<EntryModel> getAllEntries();
    void updateEntryByID(int id, DateTime timeStamp, int boarded, int leftBehind);
    void createEntry(DateTime timeStamp, int boarded, int leftBehind);
    EntryModel? findEntryByID(int id);
    void deleteEntryByID(int id);
}