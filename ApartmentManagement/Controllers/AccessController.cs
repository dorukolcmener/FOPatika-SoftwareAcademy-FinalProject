using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.DBOperations;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers;

// [Route("[controller]")]
public class AccessController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;
    private readonly TokenHandler _tokenHandler;

    public AccessController(ApartmentDBContext context, IMapper mapper, TokenHandler tokenHandler)
    {
        _context = context;
        _mapper = mapper;
        _tokenHandler = tokenHandler;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login([FromForm] LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(x => x.EMail == model.Email && x.Password == model.Password);
        if (user == null)
        {
            // Create error list
            var errorList = new List<string>();
            // Add error to error list
            errorList.Add("Invalid e-mail or password.");
            // Add error list to view data
            ViewBag.Errors = errorList;
            // Return view
            return View();
        }
        // Redirect to home
        Response.Cookies.Append("ApartmentAuth", _tokenHandler.WriteToken(user.Id));
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("ApartmentAuth");
        return RedirectToAction("Index", "Home");
    }
}
