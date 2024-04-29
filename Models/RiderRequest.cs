using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNFBusShuttle.Models
{
    [Table("RiderRequest", Schema = "dbo")]
    public class RiderRequest
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> BusNumber { get; set; }
        public Nullable<int> WaitTime { get; set; }      
        public Nullable<bool> RequestStatus { get; set; }
        public string? DriverRemarks { get; set; }
        public Nullable<bool> ActionTaken { get; set; }      
        public Nullable<System.DateTime> DateAdded { get; set; } = DateTime.Now;
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        //
        // Custome
        // 
    }
}
