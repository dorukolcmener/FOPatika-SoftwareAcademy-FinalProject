using ApartmentManagement.Entities;

namespace ApartmentManagement.Models;

public class BillCreateViewModel
{
    public string Block { get; set; }
    public int Door { get; set; }
    public double Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
    public BillType Type { get; set; }
}