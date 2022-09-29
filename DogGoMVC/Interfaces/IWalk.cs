using DogGoMVC.Models;

namespace DogGoMVC.Interfaces
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalks();
        List<Walk> GetWalksByWalker(int id);
        void CreateWalk(Walk walk);

    }
}
