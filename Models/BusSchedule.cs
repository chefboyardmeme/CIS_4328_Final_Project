using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNFBusShuttle.Models
{
    [Table("BusSchedule", Schema = "dbo")]
    public class BusSchedule
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> BusNumber { get; set; }
        public string? BusName { get; set; }
        public string? ScheduleTitle { get; set; }
        public string? Description { get; set; }         
        public Nullable<System.DateTime> DateAdded { get; set; } = DateTime.Now;
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
