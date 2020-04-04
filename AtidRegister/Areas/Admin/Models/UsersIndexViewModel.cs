using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// VM for users page
    /// </summary>
    public class UsersIndexViewModel
    {
        /// <summary>
        /// students list
        /// </summary>
        public List<BaseStudentsVM> Students { get; set; }
        /// <summary>
        /// admins list
        /// </summary>
        public List<BaseAdminVM> Admins { get; set; }
    }
    /// <summary>
    /// Basic model for storing and sending student
    /// </summary>
    public class BaseStudentsVM
    {
        public string UserName { get; set; }
        public Grade Grade { get; set; }
        public bool DidChose { get; set; }
    }
    /// <summary>
    /// Basic model for storing and sending admin
    /// </summary>
    public class BaseAdminVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
