using System;
using System.ComponentModel.DataAnnotations;

namespace AtidRegister.Areas.Admin.Models
{
    /// <summary>
    /// FAQ VM
    /// </summary>
    public class CreareFAQuestionModel
    {
        /// <summary>
        /// Question
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Question { get; set; }
        /// <summary>
        /// Answer to the question
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Answer { get; set; }
    }
}