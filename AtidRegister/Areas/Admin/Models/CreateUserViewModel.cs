using AtidRegister.Helpers;
using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class CreateUserViewModel
    {
        /// <summary>
        /// Name of new user
        /// </summary>
        [Required]
        [Display(Name = "שם")]
        public string Name { get; set; }
        /// <summary>
        /// is new user admin
        /// </summary>
        [Display(Name = "מנהל?")]
        public bool IsAdmin { get; set; }
        #region AdminOnly
        [Display(Name = "כתובת מייל (למנהל בלבד)")]
        [RequiredIf("IsAdmin", true, ErrorMessage = "יש להגדיר מייל למנהל.")]
        public string Email { get; set; }
        [RequiredIf("IsAdmin", true, ErrorMessage = "יש להגדיר סיסמה למנהל.")]
        [Display(Name = "סיסמה (למנהל בלבד)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        #endregion
        #region StudentsOnly
        [RequiredIf("IsAdmin", false, ErrorMessage = "יש להגדיר כיתה לתלמיד")]
        [Display(Name = "כיתה (לתלמיד בלבד)")]
        public int GradeId { get; set; }
        #endregion
        public List<Class> Classes { get; set; }
    }
}
