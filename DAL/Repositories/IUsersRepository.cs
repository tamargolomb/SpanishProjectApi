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
        Users? GetById(int id);
        void Update(Users User);
     
    }
}
