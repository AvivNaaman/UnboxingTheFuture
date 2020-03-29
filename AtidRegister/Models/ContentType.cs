using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    public class ContentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Content> Contents { get; set; }
    }
}
