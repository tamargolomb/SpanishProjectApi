using DAL;
using DTO;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IUserService
    {
        Users? GetById(int id);
        void Register(Users user);
        List<ShowUserDTO> GetAllUsers();
        void Update(Users user);
    }
}
