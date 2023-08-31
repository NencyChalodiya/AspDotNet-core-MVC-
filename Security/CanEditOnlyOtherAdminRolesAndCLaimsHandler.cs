﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebApplication.Security
{
	public class CanEditOnlyOtherAdminRolesAndCLaimsHandler : AuthorizationHandler<ManageAdminRoleAndClaimRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRoleAndClaimRequirement requirement)
		{
			var authorFilterContext = context.Resource as AuthorizationFilterContext;
			if (authorFilterContext == null)
			{
				return Task.CompletedTask;
			}
			string loggedInAdminId =
				context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

			string adminIdBeingEdited = authorFilterContext.HttpContext.Request.Query["userId"];

			if (context.User.IsInRole("Admin") &&
				context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
				adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}
}
