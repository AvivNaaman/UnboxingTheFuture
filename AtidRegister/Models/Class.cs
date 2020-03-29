using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    public class Class
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Class Name
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Grades of the class
        /// </summary>
        public List<Grade> Grades { get; set; }
    }
}
