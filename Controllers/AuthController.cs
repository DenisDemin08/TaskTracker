using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.ValueObject;
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
}