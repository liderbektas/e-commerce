namespace ProductManagament_MVC.Models;


public class Contact
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public int? UserId { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime ReceivedDate { get; set; } = DateTime.Now;
}