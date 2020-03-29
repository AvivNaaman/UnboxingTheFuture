using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// a Class respresenting a single timestrip
    /// </summary>
    public class TimeStrip
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Start Time
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// End Time
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// Time strip contents
        /// </summary>
        public List<Content> Contents { get; set; }
    }
}
