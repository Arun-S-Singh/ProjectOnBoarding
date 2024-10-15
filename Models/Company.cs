
using System.ComponentModel.DataAnnotations;

namespace ProjectOnBoarding.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Company Name")]
        public string CompanyName { get; set; }
        
    }
}
