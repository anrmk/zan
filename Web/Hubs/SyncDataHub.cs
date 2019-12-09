using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs {
    public class SyncDataHub: Hub {
        public SyncDataHub() {
        }

        public async Task SendDirectSyncResult(string userId, string key) {
            await Clients.User(userId).SendAsync("syncresult");
        }

        public string GetConnectionId() {
            return Context.ConnectionId;
        }

        public override async Task OnConnectedAsync() {
            var connectedId = Context.ConnectionId;

            await base.OnConnectedAsync();
        }
    }
}
