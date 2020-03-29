using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    public class EditPersonViewModel : CreatePersonViewModel
    {
        [Display(Name = "תמונה נוכחית")]
        public string OldImageFile { get; set; }
        public int Id { get; set; }
    }
}
