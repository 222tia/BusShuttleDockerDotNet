using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace View.Models
{
    public class DriverDashboardModel
    {
        public int StopId { get; set; }
        public int BusId { get; set; }
        public int LoopId { get; set; }
    }
}