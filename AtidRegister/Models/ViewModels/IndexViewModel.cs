using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<ContentCheck> Contents { get; set; }
        public string UserName { get; set; }
        public List<TimeStrip> TimeStrips { get; set; }
        public string Priorities { get; set; }
    }
    public class ContentCheck
    {
        public Content Content { get; set; }
        public bool isChecked { get; set; }
    }
}
