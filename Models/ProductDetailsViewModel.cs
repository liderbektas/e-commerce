namespace ProductManagament_MVC.Models;

public class ProductDetailsViewModel
{
    public Products Product { get; set; }
    public List<Reviews> Reviews { get; set; }
    public List<Questions> Questions { get; set; }
    public List<Answer> Answers { get; set; }
    public double RatingAverage { get; set; }
    public Dictionary<int, int> RatingCounts { get; set; }
}
