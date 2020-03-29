using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class CreatePersonViewModel
    {
        [Required(ErrorMessage = "חובה שיהיה שם.")]
        [DataType(DataType.Text)]
        [Display(Name = "שם (מלא)")]
        [StringLength(35, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "לא לשכוח תפקיד!")]
        [DataType(DataType.Text)]
        [Display(Name = "תפקיד")]
        [StringLength(60, ErrorMessage = "הי, זה יותר מדי ארוך!")]
        public string JobTitle { get; set; }
        [Display(Name = "תמונת פרופיל")]
        public IFormFile PersonImage { get; set; }
    }
}
