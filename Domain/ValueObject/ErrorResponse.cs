namespace TaskTracker.Domain.ValueObject
{
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
}
