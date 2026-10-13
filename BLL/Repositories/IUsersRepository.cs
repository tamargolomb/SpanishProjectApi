using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories
{
    public interface IUsersRepository
    {
        void Add(Users User);
        List<Users> GetAll();
        Users GetById(int id);
        void Update(Users User);
        void Delete(int id);
    }
}
