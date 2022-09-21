using DogGoMVC.Models;

namespace DogGoMVC.Interfaces
{
    public interface IDogRepository
    {
        List<Dog> GetDogsByOwnerId(int ownerId);
    }
}
