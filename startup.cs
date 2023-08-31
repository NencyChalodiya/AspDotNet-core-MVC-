using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebApplication.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace WebApplication
{
	public class startup
	{

		private IConfiguration _config;
		public startup(IConfiguration config)
		{
			_config = config;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			// Add services to the service collection

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
				_config.GetConnectionString("EmployeeDbServer")));
			// services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDbServer")));
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 3;
				options.Password.RequiredUniqueChars = 3;
				options.SignIn.RequireConfirmedEmail = true;
			}).AddEntityFrameworkStores<AppDbContext>()
			  .AddDefaultTokenProviders();

			

			services.AddMvc(option => option.EnableEndpointRouting = false);
			services.AddMvc(config =>
			{
			       var policy = new AuthorizationPolicyBuilder()
				                    .RequireAuthenticatedUser()
									.Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
			});
			services.AddAuthorization(options =>
			{
				options.AddPolicy("DeleteRolePolicy",
					policy => policy.RequireClaim("Delete Role"));

				options.AddPolicy("EditRolePolicy",
					policy => policy.AddRequirements(new ManageAdminRoleAndClaimRequirement()));

				options.AddPolicy("AdminRolePolicy",
					policy => policy.RequireRole("Admin"));
			});







			services.AddScoped<IEmployeeRespository, SQLEmployeeRepository>();
			services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndCLaimsHandler>();
			services.AddSingleton<IAuthorizationHandler,SuperAdminHandler>();
			

		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<startup> logger)
		{
			// Configure the app's middleware

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseStatusCodePagesWithReExecute("/Error/{0}");
			}



			//Use of JSON to retrive value
			//app.Run(async (context) =>
			//{
			//await context.Response.WriteAsync(_config["MyKey"]);


			//});


			//USe of middleswares

			//app.Use(async (context,next) =>
			//{
			//await context.Response.WriteAsync("Hello from 1 middleware");
			//logger.LogInformation("MW1: INcoming request");
			//await next();
			//logger.LogInformation("MW1 : Outcoming response");
			//});

			//app.Use(async (context, next) =>
			//{
			//await context.Response.WriteAsync("Hello from 1 middleware");
			//logger.LogInformation("MW2: INcoming request");
			//await next();
			//logger.LogInformation("MW2 : Outcoming response");


			//Use of static files 
			//DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
			//defaultFilesOptions.DefaultFileNames.Clear();
			//defaultFilesOptions.DefaultFileNames.Add("footer.html");
			//app.UseDefaultFiles(defaultFilesOptions);
			//app.UseStaticFiles();


			//Using fileServerOption object which considers the file as default file
			//FileServerOptions options = new FileServerOptions();
			//options.DefaultFilesOptions.DefaultFileNames.Clear();
			//options.DefaultFilesOptions.DefaultFileNames.Add("footer.html");
			//app.UseFileServer(options);

			//app.UseStaticFiles();
			//UseMvcWithDefaultRoute() is deafaault routing middleware 
			//app.UseMvcWithDefaultRoute();

			//Two types of routing method -- 1.Conventional Routing 2.Attribute routing
			//1. Using Conventional routing UseMvc() is use to add any routing capabilities
			//to have optional in id add ? last of the id and before ending of curly braces
			//To make default we can write controller=Home and action=Index
			//{controller=Home}/{action=Index}/{id?}
			//app.UseMvc(routes =>
			//{
			//routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			//});

			//2. Attribute routing
			// app.UseMvc();




			

			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
			app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Request handled and response produced");
				//logger.LogInformation("MW3: Request handled and response produced");
			});
		}

	}
}
