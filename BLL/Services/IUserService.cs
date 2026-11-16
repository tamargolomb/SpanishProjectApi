using DAL;
using DTO;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public interface IUserService
    {
        //Users? GetById(int id);
        bool Register(AddUserDTO dto);
        AuthResponseDto? Login(LoginUserDTO dto);
        List<ShowUserDTO> GetAllUsers();
        ShowUserDTO UpdateUser(int id, UpdateUserDTO dto);
        bool DeleteUser(int id);
    }
}
