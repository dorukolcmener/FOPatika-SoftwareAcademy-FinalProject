using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

[RoleAttribute]
[Route("[controller]s")]
public class ApartmentController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;

    public ApartmentController(ApartmentDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: Apartment
    [RoleAttribute("admin")]
    public IActionResult Index()
    {
        var apartmentList = _context.Apartments.Include(apartment => apartment.User).ToList();
        var apartmentViewModelList = _mapper.Map<List<ApartmentViewModel>>(apartmentList);
        return View(apartmentViewModelList);
    }

    // View apartment and bills with id
    [HttpGet("[action]/{id}")]
    public IActionResult Details(int id)
    {
        var currentUser = HttpContext.Items["User"] as User;
        var requestedApartment = _context.Apartments.Include(requestedApartment => requestedApartment.User).Where(requestedApartment => requestedApartment.Id == id).SingleOrDefault();
        if (requestedApartment == null)
            return NotFound();

        if (currentUser.Role != UserType.admin)
        {
            if (requestedApartment.UserId != currentUser.Id)
            {
                Response.StatusCode = 401;
                return Content("You are not authorized to access this page.");
            }
        }

        var billList = _context.Bills.Include(bill => bill.Apartment).Include(bill => bill.Apartment.User).Where(bill => bill.ApartmentId == id).ToList();
        var billViewModelList = _mapper.Map<List<BillViewModel>>(billList);
        var apartment = _context.Apartments.Include(apartment => apartment.User).Where(apartment => apartment.Id == id).SingleOrDefault();
        var apartMentViewModel = _mapper.Map<ApartmentViewModel>(apartment);
        apartMentViewModel.Bills = billViewModelList;

        ViewBag.Role = (int)currentUser.Role;
        return View(apartMentViewModel);
    }

    // GET: Apartment/Create
    [RoleAttribute("admin")]
    [HttpGet("[action]")]
    public IActionResult Create()
    {
        // Get all user emails as list
        var userList = _context.Users.ToList();
        var userEmailList = new List<string>();
        foreach (var user in userList)
            userEmailList.Add(user.EMail);
        return View(userEmailList);
    }

    // POST: Apartment/Create
    [RoleAttribute("admin")]
    [HttpPost("[action]")]
    public IActionResult Create([FromForm] ApartmentCreateViewModel apartmentCreateViewModel)
    {
        var user = _context.Users.Where(user => user.EMail.ToLower() == apartmentCreateViewModel.EMail.ToLower()).SingleOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        var apartment = _mapper.Map<Apartment>(apartmentCreateViewModel);
        apartment.UserId = user.Id;
        _context.Apartments.Add(apartment);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Update apartment view
    [RoleAttribute("admin")]
    [HttpGet("[action]/{id}")]
    public IActionResult Update(int id)
    {
        var apartment = _context.Apartments.Include(apartment => apartment.User).Where(apartment => apartment.Id == id).SingleOrDefault();
        if (apartment == null)
        {
            return NotFound();
        }

        var apartmentViewModel = _mapper.Map<ApartmentCreateViewModel>(apartment);
        apartmentViewModel.EMailList = _context.Users.Select(user => user.EMail).ToList();
        return View(apartmentViewModel);
    }

    // Update the apartment with id
    [RoleAttribute("admin")]
    [HttpPost("[action]/{id}")]
    public IActionResult Update(int id, [FromForm] ApartmentCreateViewModel apartmentViewModel)
    {
        var apartment = _context.Apartments.Find(id);
        if (apartment == null)
        {
            return NotFound();
        }
        var user = _context.Users.Where(user => user.EMail.ToLower() == apartmentViewModel.EMail.ToLower()).SingleOrDefault();

        apartment.UserId = user.Id;
        apartment.Block = apartmentViewModel.Block;
        apartment.Floor = apartmentViewModel.Floor;
        apartment.Door = apartmentViewModel.Door;
        apartment.FlatType = apartmentViewModel.FlatType;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    // Delete apartment view
    [RoleAttribute("admin")]
    [HttpGet("[action]/{id}")]
    public IActionResult Delete(int id)
    {
        var apartment = _context.Apartments.Include(apartment => apartment.User).Where(apartment => apartment.Id == id).SingleOrDefault();
        if (apartment == null)
        {
            return NotFound();
        }

        var apartmentViewModel = _mapper.Map<ApartmentViewModel>(apartment);
        return View(apartmentViewModel);
    }

    // Delete the apartment with id
    [RoleAttribute("admin")]
    [HttpPost("Delete/{id}")]
    public IActionResult DeleteApartment(int id)
    {
        Console.WriteLine($"{id} - asd");
        var apartment = _context.Apartments.Find(id);
        if (apartment == null)
        {
            return NotFound();
        }

        _context.Apartments.Remove(apartment);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}