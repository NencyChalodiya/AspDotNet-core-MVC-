using Microsoft.AspNetCore.Identity;

namespace WebApplication.Models
{
	public class ApplicationUser : IdentityUser
	{
        public string? City { get; set; }
    }
}
