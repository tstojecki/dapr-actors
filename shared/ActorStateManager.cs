using Dapr.Actors;
using Dapr.Actors.Client;
using Shared.Actors;

namespace Shared
{
    public class ActorStateManager : IStateManager
    {
        public async Task<State> GetStateAsync(string id)
        {
            var actorId = new ActorId(id);
            var actor = ActorProxy.Create<IStateActor>(actorId, "StateActor");

            return await actor.GetStateAsync();            
        }

        public async Task SetStateAsync(State state)
        {
            var actorId = new ActorId(state.Id);            
            var actor = ActorProxy.Create<IStateActor>(actorId, "StateActor");
            
            await actor.SetStateAsync(state);   
        }

        public async Task RemoveStateAsync(string id)
        {
            var actorId = new ActorId(id);
            var actor = ActorProxy.Create<IStateActor>(actorId, "StateActor");

            await actor.RemoveStateAsync();
        }
       
        public async Task RegisterPendingStateReminderAsync(string id)
        {
            var actorId = new ActorId(id);
            var actor = ActorProxy.Create<IStateActor>(actorId, "StateActor");

            await actor.RegisterStateReminderAsync();
        }

        public async Task UnregisterPendingStateReminderAsync(string id)
        {
            var actorId = new ActorId(id);
            var actor = ActorProxy.Create<IStateActor>(actorId, "StateActor");

            await actor.UnregisterStateReminderAsync();
        }

    }
}
