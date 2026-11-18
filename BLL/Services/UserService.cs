
using AutoMapper;
using BLL.Exceptions;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Lesson;
using DTO.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly ITokenService _tokens;
        private readonly IPasswordHasher<Users> _hasher;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        public UserService(
            IUsersRepository userRepository,
            IMapper mapper,
            ITokenService tokens,
            IPasswordHasher<Users> hasher,
            IEmailService emailService,
            IHostEnvironment environment,
            IMemoryCache cache)
        {
            _UsersRepository = userRepository;
            _mapper = mapper;
            _tokens = tokens;
            _hasher = hasher;
            _emailService = emailService;
            _environment = environment;
            _cache = cache;
        }

        private string GenerateToken()
        {
            return Convert.ToHexString(
                RandomNumberGenerator.GetBytes(32));
        }

        private string HashToken(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes);
        }


        public async Task<bool> Register(AddUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();

            if (_UsersRepository.EmailExists(email))
            {
                throw new InvalidOperationException(
                    "כתובת האימייל כבר רשומה במערכת.");
            }

            var user = new Users
            {
                UserName = dto.UserName.Trim(),
                UserEmail = email,
                Role = "User",
                JoinDate = DateTime.UtcNow,
                Score = 0,
                IsEmailVerified = false
            };

            user.UserPassword = _hasher.HashPassword(
                user, dto.UserPassword);

            string code = RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();

            user.VerificationCode = code;
            user.VerificationCodeExpiresAt =
                DateTime.UtcNow.AddMinutes(10);

            _UsersRepository.Add(user);

            if (_environment.IsDevelopment())
            {
                Console.WriteLine(
                    $"Development verification code: {code}");
            }

            bool isEmailSent = await _emailService.SendEmailAsync(
                email,
                "HolaHub - Verify your email",
                $"Your verification code is: {code}"
            );

            Console.WriteLine($"Email service result: {isEmailSent}");
            if (!isEmailSent)
            {
                throw new Exception(
                    "שליחת מייל האימות נכשלה.");
            }

            return true;
        }


        public AuthResponseDto VerifyCodeAndLogin(VerifyCodeDTO dto)
        {
            string email = dto.UserEmail.Trim().ToLowerInvariant();

            var user = _UsersRepository.GetByEmail(email);

            if (user == null)
                throw new InvalidOperationException(
                    "המשתמש לא נמצא");

            if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt <
                DateTime.UtcNow)
            {
                throw new InvalidOperationException(
                    "פג תוקפו של הקוד. יש לבקש קוד חדש.");
            }

            if (user.VerificationCode != dto.Code.Trim())
            {
                throw new InvalidOperationException(
                    "קוד האימות שגוי.");
            }

            // האימות הצליח, מנקים את הקוד
            user.IsEmailVerified = true;
            user.VerificationCode = null;
            user.VerificationCodeExpiresAt = null;

            _UsersRepository.Update(user);

            // מחזירים JWT כדי שהמשתמש יוכל להיכנס
            return _tokens.CreateToken(user);
        }

        public AuthResponseDto? Login(LoginUserDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();

            var user = _UsersRepository.GetByEmail(email);

            if (user is null)
                return null;

            var result = _hasher.VerifyHashedPassword(
                user,
                user.UserPassword,
                dto.UserPassword);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return _tokens.CreateToken(user);
        }

        public List<ShowUserDTO> GetAllUsers()
        {
            var users = _UsersRepository.GetAll();

            return _mapper.Map<List<ShowUserDTO>>(users);
        }

        public ShowUserDTO? GetByUserId(int id)
        {
            var user = _UsersRepository.GetById(id);

            if (user == null)
                return null;

            return _mapper.Map<ShowUserDTO>(user);
        }

        public async Task<ShowUserDTO?> UpdateUser(int id, UpdateUserDTO dto)
        {
            var existingUser = _UsersRepository.GetById(id);

            if (existingUser == null)
                return null;

            var newEmail = dto.UserEmail.Trim().ToLowerInvariant();

            bool emailChanged = existingUser.UserEmail != newEmail;

            if (emailChanged && _UsersRepository.EmailExists(newEmail))
            {
                throw new InvalidOperationException(
                    "כתובת האימייל כבר רשומה במערכת.");
            }

            existingUser.UserName = dto.UserName.Trim();

            if (emailChanged)
            {
                existingUser.UserEmail = newEmail;

                // כתובת המייל השתנתה ולכן צריך אימות מחדש
                existingUser.IsEmailVerified = false;

                // יצירת קוד אימות חדש בן 6 ספרות (בדיוק כמו ב-Register)
                string code = RandomNumberGenerator
                    .GetInt32(100000, 1000000)
                    .ToString();

                existingUser.VerificationCode = code;
                existingUser.VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);

                // שליחת קוד האימות (בדומה ללוגיקה ב-Register)
                if (_environment.IsDevelopment())
                {
                    Console.WriteLine(
                        "===== HOLAHUB DEVELOPMENT =====");
                    Console.WriteLine(
                        $"Verification code for updated email: {code}");
                    Console.WriteLine(
                        "===============================");
                }
                else
                {
                    bool isEmailSent = await _emailService.SendEmailAsync(
                        newEmail,
                        "HolaHub - Verify your new email",
                        $"Your verification code is: <b>{code}</b>"
                    );

                    if (!isEmailSent)
                    {
                        throw new Exception(
                            "שליחת מייל האימות נכשלה.");
                    }
                }
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
        // בקשת שחזור סיסמה (שליחת קוד בן 6 ספרות)


        public async Task ForgotPassword(ForgotPasswordDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();
            var cacheKey = $"password-reset:{email}";

            if (_cache.TryGetValue(cacheKey, out _))
                return;

            var user = _UsersRepository.GetByEmail(email);

            if (user == null)
                return;

            string code = RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();

            user.PasswordResetCodeHash = HashToken(code);
            user.PasswordResetCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);
            user.PasswordResetAttempts = 0;

            _UsersRepository.Update(user);

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("===== HOLAHUB DEVELOPMENT =====");
                Console.WriteLine($"Password reset code: {code}");
                Console.WriteLine("===============================");
            }

            bool isEmailSent = await _emailService.SendEmailAsync(
                email,
                "HolaHub - Reset your password",
                $"Your password reset code is: {code}"
            );

            if (!isEmailSent)
                throw new Exception("שליחת מייל שחזור הסיסמה נכשלה.");

            _cache.Set(cacheKey, true, TimeSpan.FromSeconds(60));
        }


        // איפוס סיסמה באמצעות הקוד

        public bool ResetPassword(ResetPasswordDTO dto)
        {
            var email = dto.UserEmail.Trim().ToLowerInvariant();
            var user = _UsersRepository.GetByEmail(email);

            if (user == null)
                return false;

            if (user.PasswordResetCodeHash == null ||
                user.PasswordResetCodeExpiresAt == null ||
                user.PasswordResetCodeExpiresAt <= DateTime.UtcNow ||
                user.PasswordResetAttempts >= 5)
            {
                return false;
            }

            string enteredCodeHash = HashToken(dto.Code.Trim());

            bool isValid = CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(user.PasswordResetCodeHash),
                Convert.FromHexString(enteredCodeHash)
            );

            if (!isValid)
            {
                user.PasswordResetAttempts++;

                if (user.PasswordResetAttempts >= 5)
                {
                    user.PasswordResetCodeHash = null;
                    user.PasswordResetCodeExpiresAt = null;
                }

                _UsersRepository.Update(user);
                return false;
            }

            user.UserPassword =
                _hasher.HashPassword(user, dto.NewPassword);

            user.PasswordResetCodeHash = null;
            user.PasswordResetCodeExpiresAt = null;
            user.PasswordResetAttempts = 0;

            _UsersRepository.Update(user);

            return true;
        }

    }
}
