using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// a Class respresenting a final schedule for user by time strip.
    /// </summary>
    public class UserSchedule
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// The User With the Scheduling
        /// </summary>
        public AppUser User { get; set; }
        /// <summary>
        /// User PK
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The Scheduled content
        /// </summary>
        public Content Content { get; set; }
        /// <summary>
        /// Content PK
        /// </summary>
        public int ContentId { get; set; }
        /// <summary>
        /// Priority by requested
        /// </summary>
        public int? Priority { get; set; }
    }
}
