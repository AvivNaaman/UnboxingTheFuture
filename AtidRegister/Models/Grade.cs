using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// a Class representing grade of class
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Number of grade in class
        /// </summary>
        public int ClassNumber { get; set; }
        /// <summary>
        /// Students of the grade
        /// </summary>
        public List<AppUser> Students { get; set; }
        /// <summary>
        /// The PK of the grade's class
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// The class of the grade
        /// </summary>
        public Class Class { get; set; }
    }
}
