using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

[RoleAttribute("admin")]
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

    public IActionResult Index()
    {
        var billList = _context.Bills.Include(bill => bill.Apartment).Include(bill => bill.Apartment.User).OrderBy(b => b.IsPaid).ToList<Bill>();
        var billViewModelList = _mapper.Map<List<BillViewModel>>(billList);
        return View(billViewModelList);
    }

    // Create Bill
    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }

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

    [HttpPost("Delete/{id}")]
    public IActionResult DeletePost([FromBody] int id)
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
}