using DogGo.Models;

namespace DogGo.Interfaces
{
    public interface IDogRepository
    {
        List<Dog> GetDogsByOwnerId(int ownerId);
    }
}
