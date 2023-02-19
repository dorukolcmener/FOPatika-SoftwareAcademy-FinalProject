using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

[Route("[controller]s")]
public class UserController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;

    public UserController(ApartmentDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: User
    public IActionResult Index()
    {
        var userList = _context.Users.OrderBy(u => u.Role).ToList();
        var userViewModelList = _mapper.Map<List<UserViewModel>>(userList);
        return View(userViewModelList);
    }

    // GET: User/Details/5
    [HttpGet("[action]/{id}")]
    public IActionResult Details(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return View(userViewModel);
    }

    // GET: User/Create
    [HttpGet("[action]")]
    public IActionResult CreateUser()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost("[action]")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateUser([FromForm] UserViewModel userViewModel)
    {
        // Map
        var user = _mapper.Map<User>(userViewModel);
        return Ok();
    }

    // GET: User/Edit/5
    [HttpGet("[action]/{id}")]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return View(userViewModel);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UserViewModel userViewModel)
    {
        if (id != userViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var user = _mapper.Map<User>(userViewModel);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(userViewModel);
    }
}