using System.ComponentModel.DataAnnotations;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
	public class EmployeeViewModel
	{
		

		[Required]
		[MaxLength(50, ErrorMessage = "Name should not exceed 50 character")]
		public string? Name { get; set; }

		[Required]
		[RegularExpression("^\\S+@\\S+$", ErrorMessage = "Invalid Email Format")]
		public string? Email { get; set; }

		[Required]
		public Dept? Department { get; set; }

		public List<IFormFile> Photos { get; set; }
	}
}
