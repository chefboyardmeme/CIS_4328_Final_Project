using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNFBusShuttle.Models
{
    [Table("DriverLocation", Schema = "dbo")]
    public class DriverLocation
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> DriverId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; } = DateTime.Now;
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
