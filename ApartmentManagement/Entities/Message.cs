namespace ApartmentManagement.Entities;

// Private message entity
public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public int FromId { get; set; }
    public User From { get; set; }
    public int ToId { get; set; }
    public User To { get; set; }
}