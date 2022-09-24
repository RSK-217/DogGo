using DogGoMVC.Models;
using System.Collections.Generic;

namespace DogGoMVC.Interfaces
{
    public interface INeighborhoodRepository
    {
        List<Neighborhood> GetAll();
    }
}
