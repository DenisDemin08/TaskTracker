using TaskTracker.Domain.ValueObject;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface ITaskCompletionConfirmationService
    {
        public interface ITaskCompletionConfirmation
        {
            /// <summary>
            /// Запросить подтверждение выполнения задачи
            /// </summary>
            Task RequestConfirmationAsync(int taskId, TeamMember requester);

            /// <summary>
            /// Подтвердить выполнение задачи
            /// </summary>
            Task ConfirmTaskCompletionAsync(int taskId, Manager confirmer, string? comment);

            /// <summary>
            /// Отклонить подтверждение выполнения задачи
            /// </summary>
            Task RejectTaskCompletionAsync(int taskId, Manager reviewer, string reason);

            /// <summary>
            /// Получить список задач, ожидающих подтверждения
            /// </summary>
            Task<List<TaskConfirmationDto>> GetPendingConfirmationsAsync(int taskId);

            /// <summary>
            /// Получить историю подтверждений для задачи
            /// </summary>
            Task<List<ConfirmationRecord>> GetConfirmationHistoryAsync(int taskId);
        }
    }
}
