using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models;

public class EntryEditModel {
    public int Id { get; set; }
    public int StopId { get; set; }
    public int LoopId { get; set; }
    public int DriverId { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }

    public static EntryEditModel FromEntry(EntryModel entry) {
        return new EntryEditModel {
            Id = entry.Id,
            StopId = entry.StopId,
            LoopId = entry.LoopId,
            DriverId = entry.DriverId,
            TimeStamp = entry.TimeStamp,
            Boarded = entry.Boarded,
            LeftBehind = entry.LeftBehind
           };
    }
}
