using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagement.Models;
using ApartmentManagement.DBOperations;
using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.Entities;

namespace ApartmentManagement.Controllers;

[RoleAttribute]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApartmentDBContext _context;

    public HomeController(ILogger<HomeController> logger, ApartmentDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var currentUser = HttpContext.Items["User"] as User;
        if (currentUser.Role == UserType.admin)
            return View();
        else
            return RedirectToAction("Details", "Users", new { id = currentUser.Id });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
