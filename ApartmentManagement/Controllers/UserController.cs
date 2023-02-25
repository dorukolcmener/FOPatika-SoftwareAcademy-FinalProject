using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;


[Route("[controller]s")]
[RoleAttribute]
public class UserController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;

    public UserController(ApartmentDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: Users
    [RoleAttribute("admin")]
    public IActionResult Index()
    {
        List<User> userList;
        userList = _context.Users.OrderBy(u => u.Role).ToList();
        var userViewModelList = _mapper.Map<List<UserViewModel>>(userList);
        return View(userViewModelList);
    }

    // GET: User/Details/5
    [HttpGet("[action]/{id}")]
    public IActionResult Details(int id)
    {
        var user = _context.Users.Find(id);
        var currentUser = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return NotFound();
        }
        if (currentUser.Id != id && currentUser.Role != UserType.admin)
        {
            Response.StatusCode = 401;
            return Content("You are not authorized to access this page.");
        }
        var userViewModel = _mapper.Map<UserViewModel>(user);
        // Get apartments
        var apartmentList = _context.Apartments.Where(a => a.UserId == id).ToList();
        var apartmentViewModelList = _mapper.Map<List<ApartmentViewModel>>(apartmentList);
        // Craete list of users's billviewmodel of apartments
        var billViewModelList = new List<BillViewModel>();
        foreach (var apartment in apartmentList)
        {
            var billList = _context.Bills.Where(b => b.ApartmentId == apartment.Id).ToList();
            var billViewModel = _mapper.Map<List<BillViewModel>>(billList);
            billViewModelList.AddRange(billViewModel);
        }
        billViewModelList = billViewModelList.OrderBy(b => b.DueDate).OrderBy(b => b.IsPaid).ToList();
        // Get vehicles of user
        var vehicleList = _context.Vehicles.Where(v => v.OwnerId == id).ToList();
        var vehicleViewModelList = _mapper.Map<List<VehicleViewModel>>(vehicleList);

        userViewModel.Apartments = apartmentViewModelList;
        userViewModel.Bills = billViewModelList;
        userViewModel.Vehicles = vehicleViewModelList;

        // ViewBag.Role = (int)currentUser.Role;
        return View(userViewModel);
    }

    // GET: User/Create
    [RoleAttribute("admin")]
    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [RoleAttribute("admin")]
    [HttpPost("[action]")]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromForm] UserViewModel userViewModel)
    {
        // Map
        var user = _mapper.Map<User>(userViewModel);
        // Save
        _context.Users.Add(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET: User/Edit/5
    [RoleAttribute("admin")]
    [HttpGet("[action]/{id}")]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        var userCreateModel = _mapper.Map<UserCreateModel>(user);
        return View(userCreateModel);
    }

    // POST: User/Edit/5
    [RoleAttribute("admin")]
    [HttpPost("[action]/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [FromForm] UserCreateModel userViewModel)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.EMail = userViewModel.Email ?? user.EMail;
        user.Name = userViewModel.Name ?? user.Name;
        user.Surname = userViewModel.Surname ?? user.Surname;
        user.Password = userViewModel.Password ?? user.Password;
        user.TCNo = userViewModel.TCNo > 0 ? userViewModel.TCNo : user.TCNo;
        user.Phone = userViewModel.Phone ?? user.Phone;
        user.Balance = userViewModel.Balance > 0 ? userViewModel.Balance : user.Balance;
        user.Role = Enum.Parse<UserType>(userViewModel.Role);

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET: User/Delete/5
    [RoleAttribute("admin")]
    [HttpGet("[action]/{id}")]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        var userViewModel = _mapper.Map<UserViewModel>(user);
        return View(userViewModel);
    }

    // POST: User/Delete/5
    [RoleAttribute("admin")]
    [HttpPost("[action]/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _context.Users.Find(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}