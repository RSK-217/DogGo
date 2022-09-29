using DogGoMVC.Models;
using DogGoMVC.Models.Filters;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGoMVC.Interfaces
{
    public interface IDogRepository 
    {
        List<Dog> GetDogs(DogFilter? filter = null);
        List<Dog> GetDogsByOwnerId(int ownerId);
        void AddDog(Dog dog);
        void UpdateDog(Dog dog);
        void DeleteDog(int id);
    }
}
