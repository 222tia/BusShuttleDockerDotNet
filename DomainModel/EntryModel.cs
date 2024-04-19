namespace DomainModel;

public class EntryModel { 
    public int Id { get; set; }
    public int StopId { get; set; }
    public int LoopId { get; set; }
    public int DriverId { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }

    public EntryModel(int id, int stopid, int loopid, int driverid, DateTime timestamp, int boarded, int leftbehind) {
        Id = id;
        StopId = stopid;
        LoopId = loopid;
        DriverId = driverid;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }

    public void Update(int stopid, int loopid, int driverid, DateTime timestamp, int boarded, int leftbehind) {
        StopId = stopid;
        LoopId = loopid;
        DriverId = driverid;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }
}