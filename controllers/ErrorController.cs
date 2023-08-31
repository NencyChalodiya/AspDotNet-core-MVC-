using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.controllers
{
	public class ErrorController : Controller
	{
		private readonly ILogger<ErrorController> logger;

		public ErrorController(ILogger<ErrorController> logger)
        {
			this.logger = logger;

		}
		[AllowAnonymous]
		[Route("Error")]
		public IActionResult Error()
		{	
			return View("Error");
		}


		[Route("Error/{statusCode}")]
		public IActionResult HttpStatusCodeHandler(int statusCode)
		{
			switch (statusCode)
			{
				case 404:
					ViewBag.ErrorMessage = "Sorry,the resouce you requested could not be found";
					break;

			}
			return View("NotFound");
		}
	}
}
