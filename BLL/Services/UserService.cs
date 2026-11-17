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

        public bool Register(AddUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();

            if (_UsersRepository.EmailExists(email))
                return false;
          

            var user = new Users
            {
                UserName = dto.UserName.Trim(),
                UserEmail = email,
                Role = "User",
                JoinDate = DateTime.UtcNow,
                Score = 0,
                LevelId = null
            };

            user.UserPassword =
                _hasher.HashPassword(user, dto.UserPassword);

            _UsersRepository.Add(user);

            return true;//?

           // return _tokens.CreateToken(user);
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
        public ShowUserDTO GetByUserId(int id)
        {
            var user = _UsersRepository.GetById(id);

            if (user == null)
                return null;

            return _mapper.Map<ShowUserDTO>(user);
        }
        public ShowUserDTO UpdateUser(int id, UpdateUserDTO dto)
        {
            var existingUser = _UsersRepository.GetById(id);
            if (existingUser == null)
                return null;

            var newEmail = dto.UserEmail.Trim().ToLowerInvariant();
            bool emailChanged =existingUser.UserEmail != newEmail;
            if (emailChanged && _UsersRepository.EmailExists(newEmail)) throw new InvalidOperationException("Email already exists.");
            existingUser.UserName = dto.UserName.Trim();
            if (emailChanged)
            {
                // כתובת המייל השתנתה ולכן צריך אימות מחדש
            }
            _UsersRepository.Update(existingUser);

            return _mapper.Map<ShowUserDTO>(existingUser);
        }
        public bool DeleteUser(int id)
        {
            return _UsersRepository.Delete(id);            
        }
    }
}
