using AtidRegister.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// a ViewModel for bulk student creation in admin.
    /// </summary>
    public class BulkCreateUsersViewModel
    {
        /// <summary>
        /// The CSV file upload content
        /// </summary>
        [Required(ErrorMessage = "נא להעלות קובץ CSV")]
        [Display(Name = "קובץ CSV מפורמט כנדרש")]
        public IFormFile csvFile { get; set; }
        /// <summary>
        /// The PK of the uploaded grade
        /// </summary>
        [Required(ErrorMessage = "חובה למלא כיתה.")]
        public int GradeId { get; set; }
        /// <summary>
        /// a List of classes that the user should choose from.
        /// </summary>
        public List<Class> Classes { get; set; }
    }
}
