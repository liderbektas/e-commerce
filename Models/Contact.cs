namespace ProductManagament_MVC.Models;

public class Contact
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
    public DateTime SentAt { get; set; } = DateTime.Now;
}
