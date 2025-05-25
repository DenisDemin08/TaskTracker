using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TaskTracker.Domain.Services.UseCases
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IConfiguration config,
        ILogger<AuthService> logger) : IAuthService
    {
        public async Task<Users> RegisterAsync(
            string email,
            string password,
            string fullName,
            UserRole role,
            MemberPosition? position)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email не может быть пустым");

            var existingUser = await unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            var user = new Users
            {
                Email = email,
                PasswordHash = HashPassword(password),
                FullName = fullName,
                Role = role,
            };

            await unitOfWork.Users.AddAsync(user);

            try
            {
                await unitOfWork.SaveChangesAsync();

                switch (role)
                {
                    case UserRole.Administrator:
                        await unitOfWork.Administrator.AddAsync(new Administrators
                        {
                            AdminId = user.UserId,
                            User = user
                        });
                        break;

                    case UserRole.Manager:
                        await unitOfWork.Manager.AddAsync(new Managers
                        {
                            ManagerId = user.UserId,
                            User = user
                        });
                        break;

                    case UserRole.Employee:
                        if (!position.HasValue)
                            throw new ArgumentException("Для сотрудника должна быть указана должность");

                        await unitOfWork.Employees.AddAsync(new Employees
                        {
                            EmployeeId = user.UserId,
                            User = user,
                            Position = position.Value
                        });
                        break;

                    default:
                        throw new ArgumentException("Недопустимая роль пользователя");
                }

                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при регистрации пользователя {Email}", email);
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("Ошибка регистрации пользователя", ex);
            }

            return user;
        }

        public async Task<Users> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Неверные учетные данные");

            var user = await unitOfWork.Users.GetByEmailAsync(email);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                logger.LogWarning("Неудачная попытка входа для {Email}", email);
                throw new UnauthorizedAccessException("Неверные учетные данные");
            }

            return user;
        }

        private string HashPassword(string password)
        {
            var saltBase64 = config["PasswordSettings:Salt"];
            if (string.IsNullOrEmpty(saltBase64))
            {
                throw new InvalidOperationException("Соль для хеширования пароля не настроена");
            }

            byte[] salt = Convert.FromBase64String(saltBase64);
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            try
            {
                return HashPassword(enteredPassword) == storedHash;
            }
            catch
            {
                return false;
            }
        }
    }
}