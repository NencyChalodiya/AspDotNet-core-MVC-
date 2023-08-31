using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
	public class CreateRoleViewModel
	{
		[Required]
        public string? RoleName { get; set; }
    }
}
