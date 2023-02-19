using ApartmentManagement.DBOperations;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Controllers;

[Route("[controller]s")]
public class VehicleController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;

    public VehicleController(ApartmentDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: Vehicle
    public IActionResult Index()
    {
        var vehicleList = _context.Vehicles.Include(vehicle => vehicle.Owner).ToList();
        var vehicleViewModelList = _mapper.Map<List<VehicleViewModel>>(vehicleList);
        return View(vehicleViewModelList);
    }

    // View vehicle and bills with id
    // [HttpGet("[action]/{id}")]
    // public IActionResult Details(int id)
    // {
    //     var billList = _context.Bills.Include(bill => bill.Vehicle).Include(bill => bill.Vehicle.User).Where(bill => bill.VehicleId == id).ToList();
    //     var billViewModelList = _mapper.Map<List<BillViewModel>>(billList);
    //     var vehicle = _context.Vehicles.Include(vehicle => vehicle.User).Where(vehicle => vehicle.Id == id).SingleOrDefault();
    //     var vehicleViewModel = _mapper.Map<VehicleViewModel>(vehicle);
    //     vehicleViewModel.Bills = billViewModelList;
    //     return View(vehicleViewModel);
    // }

    // Update the vehicle with id
    [HttpPost]
    public IActionResult Update(int id, VehicleViewModel vehicleViewModel)
    {
        var vehicle = _context.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        vehicle.LicensePlate = vehicleViewModel.LicensePlate;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}