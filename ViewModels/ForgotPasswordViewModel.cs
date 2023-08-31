using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
	public class ForgotPasswordViewModel
	{
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
