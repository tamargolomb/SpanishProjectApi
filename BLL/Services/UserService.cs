using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Lesson;
using DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly ITokenService _tokens;
        private readonly IPasswordHasher<Users> _hasher;
        private readonly IMapper _mapper;
        public UserService(IUsersRepository userRepository, IMapper mapper,ITokenService tokens, IPasswordHasher<Users> hasher)
        {
            _UsersRepository = userRepository;
            _mapper = mapper;
            _tokens = tokens;
            _hasher = hasher;
        }

        //public Users? GetById(int id)
        //{
        //    return _UsersRepository.GetById(id);
        //}

        public bool Register(AddUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();
            if (_UsersRepository.EmailExists(email))
                return false;
            var user = new Users { UserEmail = email, Role = "User" };
            user.UserPassword = _hasher.HashPassword(user, dto.UserPassword);
            _UsersRepository.Add(user);
            return true;
        }
        public AuthResponseDto? Login(LoginUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();
            var user = _UsersRepository.GetByEmail(email);
            if (user is null)
                return null;

            var result = _hasher.VerifyHashedPassword(user, user.UserPassword, dto.UserPassword);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return _tokens.CreateToken(user);
        }
        public List<ShowUserDTO> GetAllUsers()
        {
            var users = _UsersRepository.GetAll();
            return _mapper.Map<List<ShowUserDTO>>(users);
        }
        public ShowUserDTO UpdateUser(int id, UpdateUserDTO dto)
        {
            var existingUser = _UsersRepository.GetById(id);
            if (existingUser == null)
                return null;

            _mapper.Map(dto, existingUser);
            _UsersRepository.Update(existingUser);

            return _mapper.Map<ShowUserDTO>(existingUser);
        }
        public bool DeleteUser(int id)
        {
            var user = _UsersRepository.GetOwned(id);
            if (user is null)
                return false;

            _UsersRepository.Delete(id);
            return true;
        }
    }
}
