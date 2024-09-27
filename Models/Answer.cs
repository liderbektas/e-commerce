namespace ProductManagament_MVC.Models;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int  QuestionId { get; set; }
    public Questions? Questions { get; set; }
    public int userId  { get; set; }
    public User? User { get; set; }
}