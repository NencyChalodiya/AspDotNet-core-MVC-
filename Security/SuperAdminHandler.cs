using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Security
{
		public class SuperAdminHandler : AuthorizationHandler<ManageAdminRoleAndClaimRequirement>
		{
			protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
				ManageAdminRoleAndClaimRequirement req)
			{

				if (context.User.IsInRole("Super Admin"))
				{
					context.Succeed(req);
				}
				return Task.CompletedTask;
			}
		}
	
}
