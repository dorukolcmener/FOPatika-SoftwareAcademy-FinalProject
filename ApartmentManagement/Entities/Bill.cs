using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities;

public class Bill
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
    public int Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
    public BillType Type { get; set; }
}

public enum BillType
{
    monthly,
    electricity,
    water,
    gas,
    internet,
    other
}