using AtidRegister.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// a View Model for admin login.
    /// </summary>
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "נא להזין שם משתמש.")]
        [Display(Name = "שם משתמש")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        /// <summary>
        /// Password of the admin
        /// </summary>
        [Required(ErrorMessage = "אל תשכח סיסמה..")]
        [Display(Name = "סיסמה")]
        [DataType(DataType.Password)]
        [StringLength(Int32.MaxValue, MinimumLength = 4, ErrorMessage = "סיסמה חייבת להיות באורך של 4 תויים לפחות")]
        public string Password { get; set; }

    }
}
