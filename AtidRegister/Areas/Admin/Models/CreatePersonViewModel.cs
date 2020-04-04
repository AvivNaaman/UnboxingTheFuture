using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// Person creation VM
    /// </summary>
    public class CreatePersonViewModel
    {
        /// <summary>
        /// Person full name
        /// </summary>
        [Required(ErrorMessage = "חובה שיהיה שם.")]
        [DataType(DataType.Text)]
        [Display(Name = "שם (מלא)")]
        [StringLength(35, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string FullName { get; set; }
        /// <summary>
        /// Person job title
        /// </summary>
        [Required(ErrorMessage = "לא לשכוח תפקיד!")]
        [DataType(DataType.Text)]
        [Display(Name = "תפקיד")]
        [StringLength(60, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string JobTitle { get; set; }
        /// <summary>
        /// Person profile image
        /// </summary>
        [Display(Name = "תמונת פרופיל")]
        public IFormFile PersonImage { get; set; }
    }
}
