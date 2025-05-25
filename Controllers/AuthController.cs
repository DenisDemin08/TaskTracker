using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// Контроллер для управления аутентификацией и регистрацией пользователей
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Регистрация нового пользователя в системе
        /// </summary>
        /// <param name="role">Роль пользователя (должна быть из перечисления UserRole)</param>
        /// <param name="request">Данные для регистрации</param>
        /// <returns>Данные зарегистрированного пользователя</returns>
        /// <response code="200">Успешная регистрация</response>
        /// <response code="400">Неверные входные данные или роль</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        /// <remarks>
        /// Пример запроса:
        /// POST /api/auth/register/Developer
        /// {
        ///     "email": "user@example.com",
        ///     "password": "P@ssw0rd123",
        ///     "fullName": "Иванов Иван Иванович",
        ///     "position": "Backend"
        /// }
        /// </remarks>
        [HttpPost("register/{role}")]
        [SwaggerOperation(OperationId = "RegisterUser")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromRoute] string role,
            [FromBody] RegisterRequest request)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var userRole))
                return BadRequest(new ErrorResponse("Неверная роль пользователя"));

            try
            {
                var user = await authService.RegisterAsync(
                    request.Email,
                    request.Password,
                    request.FullName,
                    userRole,
                    request.Position);

                return Ok(new UserResponse(user));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse("Внутренняя ошибка сервера: " + ex.Message));
            }
        }

        /// <summary>
        /// Аутентификация пользователя в системе
        /// </summary>
        /// <param name="request">Данные для входа</param>
        /// <returns>Данные аутентифицированного пользователя</returns>
        /// <response code="200">Успешный вход</response>
        /// <response code="401">Неверные учетные данные</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        /// <remarks>
        /// Пример запроса:
        /// GET /api/auth/login?email=user@example.com&amp;password=P@ssw0rd123
        /// </remarks>
        [HttpGet("login")]
        [SwaggerOperation(OperationId = "LoginUser")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromQuery] LoginRequest request)
        {
            try
            {
                var user = await authService.LoginAsync(request.Email, request.Password);
                return Ok(new UserResponse(user));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ErrorResponse("Неверные учетные данные"));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse("Ошибка сервера: " + ex.Message));
            }
        }
    }

    #region DTOs

    /// <summary>
    /// Модель запроса для регистрации пользователя
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль (8-100 символов)
        /// </summary>
        /// <example>P@ssw0rd123</example>
        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 100 символов")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        /// <example>Иванов Иван Иванович</example>
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Должность в компании (опционально)
        /// </summary>
        /// <example>Backend</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MemberPosition? Position { get; set; }
    }

    /// <summary>
    /// Модель запроса для аутентификации
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        /// <example>P@ssw0rd123</example>
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// Модель ответа с данными пользователя
    /// </summary>
    public class UserResponse(Users user)
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>123</example>
        public int UserId { get; } = user.UserId;

        /// <summary>
        /// Электронная почта
        /// </summary>
        /// <example>user@example.com</example>
        public string Email { get; } = user.Email;

        /// <summary>
        /// Полное имя
        /// </summary>
        /// <example>Иванов Иван Иванович</example>
        public string FullName { get; } = user.FullName;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        /// <example>Developer</example>
        public UserRole Role { get; } = user.Role;
    }

    /// <summary>
    /// Модель ответа с ошибкой
    /// </summary>
    public class ErrorResponse(string message)
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        /// <example>Неверные учетные данные</example>
        public string Message { get; } = message;
    }

    #endregion
}