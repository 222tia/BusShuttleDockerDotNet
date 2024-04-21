using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace View.Models {
    public class FilterReportsModel {
        public int BusId { get; set; }
        public int LoopId { get; set; }
        public int DriverId { get; set; }
        public int StopId { get; set; }
    }
}