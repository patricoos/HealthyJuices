using System.Security.Claims;
using HealthyJuices.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Tests.EndToEnd.Extensions
{
    internal static class BaseControllerExtensions
    {
        public static BaseApiController SetUserId(this BaseApiController controller, string userId)
        {
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }, "mock"));

            if (controller.ControllerContext.HttpContext == null)
            {
                controller.ControllerContext.HttpContext = new DefaultHttpContext { User = userClaims };
                return controller;
            }

            controller.ControllerContext.HttpContext.User = userClaims;
            return controller;
        }
    }
}