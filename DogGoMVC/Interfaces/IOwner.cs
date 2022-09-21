using DogGoMVC.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGoMVC.Interfaces
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        void AddOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(int ownerId);

    }
}

