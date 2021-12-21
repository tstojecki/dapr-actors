using Dapr.Actors;

namespace Shared.Actors
{
    public interface IStateActor : IActor
    {
        Task<State> GetStateAsync();
        Task SetStateAsync(State state);
        Task RemoveStateAsync();
        Task RegisterStateReminderAsync();
        Task UnregisterStateReminderAsync();
    }
}
