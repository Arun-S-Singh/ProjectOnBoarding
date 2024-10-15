using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectOnBoarding.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]
        [Display(Name = "Project Name")]
        public Project Project { get; set; }
    }
}
