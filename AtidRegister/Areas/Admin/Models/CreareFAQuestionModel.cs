using System;
using System.ComponentModel.DataAnnotations;

namespace AtidRegister.Areas.Admin.Models
{
    public class CreareFAQuestionModel
    {
        [Required]
        [StringLength(100)]
        public string Question { get; set; }
        [Required]
        [StringLength(500)]
        public string Answer { get; set; }
    }
}