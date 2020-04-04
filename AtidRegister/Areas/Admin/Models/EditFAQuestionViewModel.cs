using System;
using System.ComponentModel.DataAnnotations;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// Edit FAQ VM
    /// </summary>
    public class EditFAQuestionViewModel
    {
        /// <summary>
        /// PK for db access
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Question
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Question { get; set; }
        /// <summary>
        /// Answer for the question
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Answer { get; set; }
    }
}