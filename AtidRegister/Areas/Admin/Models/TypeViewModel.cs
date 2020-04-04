using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// a ViewModel for content type creation/edit
    /// </summary>
    public class TypeViewModel
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Type name/definition
        /// </summary>
        [Required(ErrorMessage = "חובה לתת שם.")]
        [Display(Name = "שם")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "אורך השם לא פחות מ3 תווים אך לא יותר מ20.")]
        public string Name { get; set; }
    }
}
