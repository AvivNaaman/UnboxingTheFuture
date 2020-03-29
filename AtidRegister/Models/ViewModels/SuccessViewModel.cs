using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models.ViewModels
{
    public class SuccessViewModel
    {
        public List<TimeStrip> TimeStrips { get; set; }
        public List<ContentUser> ContentUsers { get; set; }
    }
}
