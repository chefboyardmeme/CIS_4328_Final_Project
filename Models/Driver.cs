using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNFBusShuttle.Models
{
    [Table("Driver", Schema = "dbo")]
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Nullable<int> BusNumber { get; set; }
        public string? Email { get; set; }
        public Nullable<bool> IsActive { get; set; } = true;
        public Nullable<System.DateTime> DateAdded { get; set; } = DateTime.Now;
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        //
        // Custome
        [NotMapped]
        public string? DriverStatus { get; set; }

        [NotMapped]
        public string? DriverName
        {
            get
            {
                return (FirstName + " " + LastName);
            }
        }

    }

}
