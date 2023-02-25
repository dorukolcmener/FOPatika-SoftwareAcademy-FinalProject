using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;

namespace ApartmentManagement.AuthorizationOperations;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenHandler _tokenHandler;

    public RoleMiddleware(RequestDelegate next, TokenHandler tokenHandler)
    {
        _next = next;
        _tokenHandler = tokenHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Validate token
        if (context.Request.Cookies["ApartmentAuth"] != null)
        {
            try
            {
                var userId = _tokenHandler.ReadToken(context.Request.Cookies["ApartmentAuth"]);
                // Get user from database
                var user = context.RequestServices.GetService<ApartmentDBContext>().Users.FirstOrDefault(x => x.Id == userId);
                // Set user to view data
                context.Items["User"] = user;
                context.Items["Role"] = (int)user.Role;
                context.Items["Balance"] = user.Balance;
            }
            catch (Exception e)
            {
                context.Response.Cookies.Delete("ApartmentAuth");
                context.Response.Redirect("/Access/Logout");
                return;
            }
        }

        var roleAttribute = context?.GetEndpoint()?.Metadata?.GetMetadata<RoleAttribute>();

        // Pass through if no role attribute required for the endpoint
        if (roleAttribute == null)
        {
            await _next(context);
            return;
        }

        // Get role from the attribute
        var role = roleAttribute.Role;

        // If user is logged in
        if (context.Request.Cookies["ApartmentAuth"] != null)
        {
            //Get user
            var user = context.Items["User"] as User;

            //Check if user has the required role
            if ((int)user.Role <= role)
                await _next(context);
            //If user does not have the required role
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("You are not authorized to access this page.");
            }
        }
        //If user is not logged in
        else
        {
            context.Response.Redirect("/Access/Login");
        }
    }
}