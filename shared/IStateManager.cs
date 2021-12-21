namespace Shared
{
    public interface IStateManager
    {
        Task<State> GetStateAsync(string id);
        Task SetStateAsync(State state);
        Task RemoveStateAsync(string id);
        Task RegisterPendingStateReminderAsync(string id);
        Task UnregisterPendingStateReminderAsync(string id);
    }
}
