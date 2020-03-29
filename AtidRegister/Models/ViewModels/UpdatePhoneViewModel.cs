using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models.ViewModels
{
    public class UpdatePhoneViewModel
    {
        [Required]
        public string UserId { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "מספר טלפון - 10 ספרות (לדוגמה: 0554433221)")]
        [StringLength(10, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
