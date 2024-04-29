using System;
using Microsoft.AspNetCore.Identity;

namespace UNFBusShuttle.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int UserPk { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserId { get; set; }
        public string? UserType { get; set; }

    }
}
