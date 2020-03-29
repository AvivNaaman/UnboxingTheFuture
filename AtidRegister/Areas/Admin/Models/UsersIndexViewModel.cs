using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class UsersIndexViewModel
    {
        public List<BaseStudentsVM> Students { get; set; }
        public List<BaseAdminVM> Admins { get; set; }
    }
    public class BaseStudentsVM
    {
        public string UserName { get; set; }
        public Grade Grade { get; set; }
        public bool DidChose { get; set; }
    }
    public class BaseAdminVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
