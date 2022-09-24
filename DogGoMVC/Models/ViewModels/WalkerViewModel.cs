namespace DogGoMVC.Models.ViewModels
{
    public class WalkerViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; } 
        public int TotalWalkDuration => Walks.Sum(w => w.Duration);

    }
}
