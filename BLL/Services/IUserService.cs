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
        bool Register(AddUserDTO dto);
        AuthResponseDto? Login(LoginUserDTO dto);
        List<ShowUserDTO> GetAllUsers();
        void Update(Users user);
    }
}
