namespace DomainModel;

public class EntryModel { 
    public int Id { get; set; }
    public int StopId { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }

    public EntryModel(int id, int stopid, DateTime timestamp, int boarded, int leftbehind) {
        Id = id;
        StopId = stopid;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }

    public void Update(int stopid, DateTime timestamp, int boarded, int leftbehind) {
        StopId = stopid;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }
}