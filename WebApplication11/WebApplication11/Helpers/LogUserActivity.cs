using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingApplication.API.Data.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ShoppingApplication.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            var users = await repo.GetUser(userId);
            // updating the last active time of the user
            users.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}
