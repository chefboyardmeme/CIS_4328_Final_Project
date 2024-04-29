using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNFBusShuttle.Models
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string? UserType { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        //
        // Custome
        [NotMapped]
        public string? UserStatus { get; set; }

        [NotMapped]
        public string? UserName
        {
            get
            {
                return (FirstName + " " + LastName);
            }
        }
    }
}
