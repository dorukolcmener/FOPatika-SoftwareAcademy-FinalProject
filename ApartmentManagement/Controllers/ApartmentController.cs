using ApartmentManagement.DBOperations;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

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
        var billList = _context.Bills.Include(bill => bill.Apartment).Include(bill => bill.Apartment.User).Where(bill => bill.ApartmentId == id).ToList();
        var billViewModelList = _mapper.Map<List<BillViewModel>>(billList);
        var apartment = _context.Apartments.Include(apartment => apartment.User).Where(apartment => apartment.Id == id).SingleOrDefault();
        var apartMentViewModel = _mapper.Map<ApartmentViewModel>(apartment);
        apartMentViewModel.Bills = billViewModelList;
        return View(apartMentViewModel);
    }

    // Update the apartment with id
    [HttpPost]
    public IActionResult Update(int id, ApartmentViewModel apartmentViewModel)
    {
        var apartment = _context.Apartments.Find(id);
        if (apartment == null)
        {
            return NotFound();
        }

        apartment.Block = apartmentViewModel.Block;
        apartment.Floor = apartmentViewModel.Floor;
        apartment.Door = apartmentViewModel.Door;
        apartment.FlatType = apartmentViewModel.FlatType;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    // Delete apartment view
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