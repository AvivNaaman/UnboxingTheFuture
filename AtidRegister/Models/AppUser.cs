using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// an <see cref="Microsoft.AspNetCore.Identity.IdentityUser"/> Extensions model for the app.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// List of chosen contents
        /// </summary>
        public List<Content> Contents { get; set; }
        /// <summary>
        /// Friendly Name
        /// </summary>
        public string FriendlyName { get; set; }
        public List<ContentUser> ContentUsers { get; set; }
        /// <summary>
        /// Grade PK
        /// </summary>
        public int? GradeId { get; set; }
        public Grade Grade { get; set; }
        public List<UserSchedule> Schedules { get; set; }
    }
}
