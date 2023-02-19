using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

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
}