using ApartmentManagement.Entities;

namespace ApartmentManagement.Models;

public class ApartmentViewModel
{
    public int Id { get; set; }
    public string Block { get; set; }
    public int Floor { get; set; }
    public int Door { get; set; }
    public string FlatType { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string Phone { get; set; }
    public List<BillViewModel>? Bills { get; set; }
}