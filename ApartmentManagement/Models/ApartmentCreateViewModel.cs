using ApartmentManagement.Entities;

namespace ApartmentManagement.Models;

public class ApartmentCreateViewModel
{
    public string Block { get; set; }
    public int Floor { get; set; }
    public int Door { get; set; }
    public string FlatType { get; set; }
    public string? EMail { get; set; }
    public List<string>? EMailList { get; set; }
}