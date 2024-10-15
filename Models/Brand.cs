using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectOnBoarding.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Brand Name")]
        public string Name { get; set; }

        public int DivisionId { get; set; }
        [ForeignKey("DivisionId")]
        [ValidateNever]
        [Display(Name ="Division Name")]
        public Division Division { get; set; }
    }
}
