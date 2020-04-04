using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// Edit person VM
    /// </summary>
    public class EditPersonViewModel : CreatePersonViewModel
    {
        /// <summary>
        /// Editing image is available only.
        /// </summary>
        [Display(Name = "תמונה נוכחית")]
        public string OldImageFile { get; set; }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
    }
}
