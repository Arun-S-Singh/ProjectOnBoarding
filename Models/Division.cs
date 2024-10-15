using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOnBoarding.Models
{
    public class Division
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Division Name")]
        public string Name { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        [Display(Name ="Company Name")]
        public Company Company { get; set; }
    }
}
