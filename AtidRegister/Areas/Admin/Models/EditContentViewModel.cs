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
    /// a ViewModel class for editing existing content
    /// </summary>
    public class EditContentViewModel
    {
        /// <summary>
        /// The content PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of the content
        /// </summary>
        [Required(ErrorMessage = "הי, השדה הזה חובה!")]
        [Display(Name = "פירוט")]
        public string Title { get; set; }
        /// <summary>
        /// The description of the content
        /// </summary>
        [Required(ErrorMessage = "הי, השדה הזה חובה!")]
        [Display(Name = "פירוט")]
        [StringLength(350, ErrorMessage = "הי, זה יותר מדי ארוך")]
        public string Description { get; set; }
        #region imageStuff
        /// <summary>
        /// The image upload form field
        /// </summary>
        public IFormFile NewImage { get; set; }
        /// <summary>
        /// The old image of the content
        /// </summary>
        public string OldImageName { get; set; }
        /// <summary>
        /// The people in the content 
        /// </summary>
        #endregion
        public List<PersonCheck> People { get; set; }
        public List<TimeStrip> TimeStrips { get; set; }
        [Required]
        public int TimeStripId { get; set; }
    }
}
