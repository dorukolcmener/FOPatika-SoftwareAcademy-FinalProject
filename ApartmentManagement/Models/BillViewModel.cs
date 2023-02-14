using ApartmentManagement.Entities;

namespace ApartmentManagement.Models;

public class BillViewModel
{
    public int Id { get; set; }
    public string Block { get; set; }
    public int Floor { get; set; }
    public int Door { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public int Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
    public BillType Type { get; set; }
}