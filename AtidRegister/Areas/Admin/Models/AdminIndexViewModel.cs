using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class AdminIndexViewModel
    {
        /// <summary>
        /// Percents of registered users (who chose their perferences)
        /// </summary>
        public int UsersRegistered { get; set; }
        public int UnregisteredUsers { get; set; }
        public List<Tuple<Content, int>> ContentsWithRegCounts { get; internal set; }
        public int HasPhoneNumber { get; internal set; }
    }
}
