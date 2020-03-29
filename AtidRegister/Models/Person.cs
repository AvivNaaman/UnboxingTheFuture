using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// a Class respresenting person of content
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [Display(Name = "מזהה")]
        public int Id { get; set; }
        /// <summary>
        /// Full name
        /// </summary>
        [Display(Name = "שם מלא")]
        public string FullName { get; set; }
        /// <summary>
        /// Job title
        /// </summary>
        [Display(Name = "תפקיד")]
        public string JobTitle { get; set; }
        /// <summary>
        /// Base-64 image string
        /// </summary>
        [Display(Name = "תמונה")]
        public string ImageFile { get; set; }

        public List<ContentPerson> ContentPeople { get; set; }
    }
}
