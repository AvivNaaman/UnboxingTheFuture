using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models.ViewModels
{
    /// <summary>
    /// View model for student login
    /// </summary>
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false)]
        /// <summary>
        /// PK of user
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Classes include grades & students of grades
        /// </summary>
        public List<Class> Classes { get; set; }
    }
}
