using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.controllers
{
    //[Route("Home")]
    // [Route("[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        private IEmployeeRespository _employeeRespository;
        private IWebHostEnvironment hostingEnvironment;
        public HomeController(IEmployeeRespository employeeRespository,
                             IWebHostEnvironment hostingEnvironment)
        {
            _employeeRespository = employeeRespository;
            this.hostingEnvironment = hostingEnvironment;
        }

        //[Route("")]
        //[Route("Home")]
        //[Route("Home/Index")]
        //[Route("Index")]
        //[Route("[action]")]
        //[Route("~/")]
        [AllowAnonymous]
        public ViewResult Index()
        {
            var model =  _employeeRespository.GetAllEmployees();
            return View(model);

        }

        //[Route("Home/Details/{id}")]
        //[Route("[action]/{id}")]
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRespository.GetEmployee(id.Value);
            if(employee == null)
            {
                Response.StatusCode=404;
                return View("EmployeeNotFound",id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"

            };
           // Employee model = _employeeRespository.GetEmployee(1);

              //Using viewData we can store string data
            //ViewData["Employee"] = model;
            //ViewData["PageTitle"] = "Employees Details Using ViewData";

              //Using ViewBage we can store dynamically data
            //ViewBag.Employee = model;
            //ViewBag.PageTitle = "Employess details Using ViewBag";
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        
        public ViewResult Create() { 
            return View();
        }

		[HttpGet]
		
		public ViewResult Edit(int id)
		{
            Employee employee = _employeeRespository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath=employee.PhotoPath
            };
			return View(employeeEditViewModel);
		}

		[HttpPost]
		
		public IActionResult Create(EmployeeViewModel model)
		{
            if (ModelState.IsValid)
            {
				string UniqueFileName = ProcessUploadedFile(model);

				Employee newEmployee = new Employee
                {
                    Name =model.Name,
                    Email =model.Email,
                    Department =model.Department,
                    PhotoPath = UniqueFileName
                };
                _employeeRespository.Add(newEmployee);
				return RedirectToAction("details", new { id = newEmployee.Id });
			}
            else
            {
                return View();
            }
        }
		[HttpPost]
		
		public IActionResult Edit(EmployeeEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				Employee employee = _employeeRespository.GetEmployee(model.Id);
				employee.Name = model.Name;
				employee.Email = model.Email;
				employee.Department = model.Department;
                if(model.Photos != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                       string filePath= Path.Combine(hostingEnvironment.WebRootPath,
                                      "images" , model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
					employee.PhotoPath = ProcessUploadedFile(model);
				}
				
				_employeeRespository.Update(employee);
				return RedirectToAction("index");
			}
			else
			{
				return View();
			}
		}

		private string ProcessUploadedFile(EmployeeViewModel model)
		{
			string UniqueFileName = null;
			if (model.Photos != null && model.Photos.Count > 0)
			{
				foreach (IFormFile photo in model.Photos)
				{
					string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
					UniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
					string filePath = Path.Combine(UploadFolder, UniqueFileName);
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
						photo.CopyTo(fileStream);
					}
					

				}

			}

			return UniqueFileName;
		}
	}
}
