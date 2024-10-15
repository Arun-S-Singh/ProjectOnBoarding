using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectOnBoarding.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string UniqueFileName { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]
        [Display(Name = "Project Name")]
        public Project Project { get; set; }
    }
}
