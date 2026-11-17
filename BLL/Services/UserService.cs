using AutoMapper;
using BLL.Exceptions;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Lesson;
using DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly ITokenService _tokens;
        private readonly IPasswordHasher<Users> _hasher;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;

        public UserService(IUsersRepository userRepository, IMapper mapper,ITokenService tokens, IPasswordHasher<Users> hasher, EmailService emailService)
        {
            _UsersRepository = userRepository;
            _mapper = mapper;
            _tokens = tokens;
            _hasher = hasher;
            _emailService = emailService;
        }
        //----- יצירת טוקן אקראי
        private string GenerateToken()
        {
            return Convert.ToHexString(
                RandomNumberGenerator.GetBytes(32));
        }

        // -----שמירת Hash של הטוקן
        private string HashToken(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes);
        }




        public async Task<bool> Register(AddUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();

            // 1. בדיקה שהמייל לא תפוס
            if (_UsersRepository.EmailExists(email))
            {
                throw new InvalidOperationException("כתובת האימייל כבר רשומה במערכת.");
            }

            // 2. יצירת אובייקט המשתמש עם כל הפרטים שהזין
            var user = new Users
            {
                UserName = dto.UserName.Trim(),
                UserEmail = email,
                Role = "User",
                JoinDate = DateTime.UtcNow,
                Score = 0,
                IsEmailVerified = false
            };

            // הצפנת הסיסמה 
            user.UserPassword = _hasher.HashPassword(user, dto.UserPassword);

            //  יצירת קוד אימות בן 6 ספרות ותוקף ל-10 דקות
            string code = new Random().Next(100000, 999999).ToString();
            user.VerificationCode = code;
            user.VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);

            //  שמירה במסד הנתונים
            _UsersRepository.Add(user);

            // שליחת הקוד למייל
            bool isEmailSent = await _emailService.SendEmailAsync(
                email,
                "Spanish Learning - Verify your email",
                $"Your verification code is: <b>{code}</b>"
            );

            if (!isEmailSent)
            {
                throw new Exception("שליחת מייל האימות נכשלה.");
            }

            return true; // ההרשמה נקלטה, כעת יש לעבור למסך הזנת הקוד
        }




        //----אחרי הזנת הקוד החד פעמי
        public AuthResponseDto VerifyCodeAndLogin(VerifyCodeDTO dto)
        {
            string email = dto.UserEmail.Trim().ToLowerInvariant();
            var user = _UsersRepository.GetByEmail(email);

            if (user == null)
                throw new InvalidOperationException("המשתמש לא נמצא");

            // בדיקת תוקף והשוואת הקוד
            if ( user.VerificationCodeExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("פג תוקפו של הקוד. יש לבקש קוד חדש.");
            }

            if (user.VerificationCode != dto.Code.Trim())
            {
                throw new InvalidOperationException("קוד האימות שגוי.");
            }

            // הקוד תקין! מעדכנים שהמייל מאומת ומאפסים את הקוד
            user.IsEmailVerified = true;
            user.VerificationCode = null;
            user.VerificationCodeExpiresAt = null;

            _UsersRepository.Update(user);

            // מחזירים את הטוקן כך שהמשתמש נכנס מיד לאתר!
            return _tokens.CreateToken(user);
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
            if (emailChanged && _UsersRepository.EmailExists(newEmail)) 
                throw new InvalidOperationException("Email already exists.");

            _mapper.Map(dto, existingUser);
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

        public async Task<bool> UpdateUserLevelAsync(
            int userId,
            int levelId,
            CancellationToken ct)
        {
            return await _UsersRepository.UpdateUserLevelAsync(
                userId,
                levelId,
                ct);
        }

    }
}
