using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface IUsersRepository
    {
        void Add(Users User);
        List<Users> GetAll();
        Users? GetByEmail(string email);
        Users? GetById(int id);
        bool EmailExists(string email);
        void Update(Users User);
     
    }
}
