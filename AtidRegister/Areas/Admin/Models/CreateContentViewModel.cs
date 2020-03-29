using AtidRegister.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class CreateContentViewModel
    {
        /// <summary>
        /// The title of the content
        /// </summary>
        [Required(ErrorMessage = "חובה שיהיה שם.")]
        [DataType(DataType.Text)]
        [Display(Name = "כותרת\\שם")]
        [StringLength(60, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string Title { get; set; }
        /// <summary>
        /// The description of the content
        /// </summary>
        [Required(ErrorMessage = "לא לשכוח תפקיד!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "פירוט")]
        [StringLength(350, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string Description { get; set; }
        /// <summary>
        /// The PK of the content type
        /// </summary>
        [Display(Name = "סוג")]
        [Required]
        public int TypeId { get; set; }
        /// <summary>
        /// The image form content
        /// </summary>
        [Display(Name = "תמונה (לא חובה)")]
        public IFormFile Image { get; set; }
        /// <summary>
        /// The list of all the people
        /// </summary>
        public List<PersonCheck> People { get; set; }
        /// <summary>
        /// The list of all the content types
        /// </summary>
        public List<ContentType> Types { get; set; }
        [Required]
        public int TimeStripId { get; set; }
        public List<TimeStrip> TimeStrips { get; set; }
    }
    public class PersonCheck
    {
        /// <summary>
        /// Indicates whether person is selected
        /// </summary>
        public bool isChecked { get; set; }
        /// <summary>
        /// The person
        /// </summary>
        public Person Person { get; set; }
    }
}
