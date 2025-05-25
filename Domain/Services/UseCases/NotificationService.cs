using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject.Configuration;
using TaskTracker.Storage.Repository;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис уведомлений
    /// </summary>
    public class NotificationService(
        IOptions<SmtpSettings> smtpSettings,
        IUnitOfWork unitOfWork,
        ILogger<NotificationService> logger) : INotificationService
    {
        private readonly SmtpSettings settings = smtpSettings.Value;

        /// <inheritdoc/>
        public async Task NotifyAssigneeAsync(Tasks task, Users assignee)
        {
            var subject = $"Новая задача: {task.Title}";
            var body = $"Добрый день, {assignee.FullName}!\n\nВам назначена задача: {task.Title}\nОписание: {task.TaskDescription}";
            await SendEmailAsync(assignee.Email, subject, body);
        }

        /// <inheritdoc/>
        public async Task SendTaskUpdateNotificationAsync(Tasks task, string updateType)
        {
            if (task.AssigneeId == null) return;

            var assignee = await unitOfWork.Users.GetByIdAsync(task.AssigneeId.Value);
            if (assignee == null) return;

            var subject = $"Обновление задачи: {task.Title}";
            var body = $"Задача '{task.Title}' была обновлена.\nТип изменения: {updateType}";
            await SendEmailAsync(assignee.Email, subject, body);
        }

        /// <inheritdoc/>
        public async Task SendConfirmationAlertAsync(Tasks task, Managers responsible)
        {
            var subject = $"Требуется подтверждение задачи: {task.Title}";
            var body = $"Уважаемый {responsible.User.FullName},\n\nТребуется ваше подтверждение выполнения задачи: {task.Title}";
            await SendEmailAsync(responsible.User.Email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = settings.UseSSL,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };

            var mail = new MailMessage(settings.FromEmail, to, subject, body);

            try
            {
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка отправки email на {Email}", to);
                throw;
            }
        }
    }
}