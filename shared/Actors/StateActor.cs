using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;

namespace Shared.Actors
{
    public class StateActor : Actor, IStateActor, IRemindable
    {
        const string REMINDER_NAME = "StateReminder";
        const string STATE_NAME = "state";

        public StateActor(ActorHost host)
            : base(host)
        {
        }

        public async Task<State> GetStateAsync()
        {
            return (await StateManager.TryGetStateAsync<State>(STATE_NAME)).Value;
        }

        public async Task SetStateAsync(State state)
        {
            await StateManager.SetStateAsync(STATE_NAME, state);
        }

        public async Task RemoveStateAsync()
        {
            await StateManager.RemoveStateAsync(STATE_NAME);
        }

        public async Task RegisterStateReminderAsync()
        {
            await RegisterReminderAsync(REMINDER_NAME, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
        }

        public async Task UnregisterStateReminderAsync()
        {
            await UnregisterReminderAsync(REMINDER_NAME);
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName == REMINDER_NAME)
            {
                await HandleStateReminderAsync();
            }
        }

        async Task HandleStateReminderAsync()
        {
            Logger.LogDebug("{reminderName} reminder invoked", REMINDER_NAME);

            // TODO: remove clear cache once bug #784 has been fixed https://github.com/dapr/dotnet-sdk/issues/784
            await StateManager.ClearCacheAsync();
            var state = await StateManager.GetStateAsync<State>(STATE_NAME);

            Logger.LogDebug("{@state}", state);
        }
    }      
}
