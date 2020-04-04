using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// ViewModel for admin home. mostly stats.
    /// </summary>
    public class AdminIndexViewModel
    {
        /// <summary>
        /// Percents of registered users (who chose their perferences)
        /// </summary>
        public int UsersRegistered { get; set; }
        /// <summary>
        /// Percents of unregistered users (who chose their perferences)
        /// </summary>
        public int UnregisteredUsers { get; set; }
        /// <summary>
        /// Each contents and it's registered students num
        /// </summary>
        public List<Tuple<Content, int>> ContentsWithRegCounts { get; internal set; }
        /// <summary>
        /// How many registered has phone number in the system
        /// </summary>
        public int HasPhoneNumber { get; internal set; }
    }
}
