using Microsoft.AspNetCore.Identity;

namespace ProjectOnBoarding.Data
{
    public class ProjectDBUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
