using ApartmentManagement.Entities;

namespace ApartmentManagement.Models;

public class BillCreateViewModel
{
    public int? ApartmentId { get; set; }
    public string Block { get; set; }
    public int Door { get; set; }
    public double Amount { get; set; }
    public string DueDate { get; set; }
    public bool? IsPaid { get; set; }
    public string Type { get; set; }
}