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
public class BillController : Controller
{
    private readonly IMapper _mapper;
    private readonly ApartmentDBContext _context;
    private readonly ILogger<BillController> _logger;

    public BillController(ApartmentDBContext context, IMapper mapper, ILogger<BillController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }


    [RoleAttribute("admin")]
    public IActionResult Index()
    {
        var billList = _context.Bills.Include(bill => bill.Apartment).Include(bill => bill.Apartment.User).OrderBy(b => b.IsPaid).ToList<Bill>();
        var billViewModelList = _mapper.Map<List<BillViewModel>>(billList);
        return View(billViewModelList);
    }


    [RoleAttribute("admin")]
    // Create Bill
    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }


    [RoleAttribute("admin")]
    [HttpPost("[action]")]
    public IActionResult Create([FromForm] BillCreateViewModel billCreateViewModel)
    {
        // Get apartment with block and door
        var apartment = _context.Apartments.Include(a => a.User).Where(a => a.Block == billCreateViewModel.Block && a.Door == billCreateViewModel.Door).FirstOrDefault();

        if (apartment == null)
        {
            return NotFound();
        }

        billCreateViewModel.IsPaid = false;
        billCreateViewModel.ApartmentId = apartment.Id;
        var bill = _mapper.Map<Bill>(billCreateViewModel);
        _context.Bills.Add(bill);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }


    [RoleAttribute("admin")]
    // Get Bill Delete
    [HttpGet("[action]/{id}")]
    public IActionResult Delete(int id)
    {
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }
        var billViewModel = _mapper.Map<BillViewModel>(bill);
        return View(billViewModel);
    }


    [RoleAttribute("admin")]
    [HttpPost("Delete/{id}")]
    public IActionResult DeletePost([FromForm] int id)
    {
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }
        _context.Bills.Remove(bill);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Pay bill
    [HttpGet("[action]/{id}")]
    public IActionResult Pay(int id)
    {
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }
        var billViewModel = _mapper.Map<BillViewModel>(bill);
        return View(billViewModel);
    }

    // Pay bill post
    [HttpPost("Pay/{id}")]
    public IActionResult PaytheBill([FromForm] int id)
    {
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }

        var currentUser = HttpContext.Items["User"] as User;
        if (currentUser.Balance < bill.Amount)
            return BadRequest();

        currentUser.Balance -= bill.Amount;
        bill.IsPaid = true;
        _context.SaveChanges();
        // Redirect to home index
        return RedirectToAction("Index", "Home");
    }
}