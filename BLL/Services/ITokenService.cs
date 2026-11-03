using DAL;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ITokenService
    {
        AuthResponseDto CreateToken(Users user);
    }
}
