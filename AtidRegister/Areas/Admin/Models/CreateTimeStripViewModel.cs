using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// a ViewModel for creating TimeStrip
    /// </summary>
    public class CreateTimeStripViewModel
    {
        /// <summary>
        /// Minute of start time
        /// </summary>
        [Display(Name = "שעת התחלה")]
        [Required(ErrorMessage = "לא לשכוח דקת התחלה!")]
        [Range(0,59, ErrorMessage = "דקה יכולה להיות 0 ולכל היותר 59.")]
        public int StartMinute { get; set; }
        /// <summary>
        /// Minute of end time
        /// </summary>
        [Required(ErrorMessage = "לא לשכוח דקת סיום!")]
        [Display(Name = "שעת סיום")]
        [Range(0, 59, ErrorMessage = "דקה יכולה להיות 0 ולכל היותר 59.")]

        public int EndMinute { get; set; }
        /// <summary>
        /// Hour of start time
        /// </summary>
        [Required(ErrorMessage = "לא לשכוח שעת התחלה!")]
        [Range(8, 17, ErrorMessage = "שעת התחלה לכל הפחות 8 ולכל היותר 17.")]
        public int StartHour { get; set; }
        /// <summary>
        /// Hour of end time
        /// </summary>
        [Required(ErrorMessage = "לא לשכוח שעת סיום!")]
        [Range(8, 17, ErrorMessage = "שעת התחלה לכל הפחות 8 ולכל היותר 17.")]
        public int EndHour { get; set; }

    }
}
