using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// a Class respresenting a FAQ question + answer
    /// </summary>
    public class FAQuestion
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Question Content
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// Answer Content
        /// </summary>
        public string Answer { get; set; }
    }
}
