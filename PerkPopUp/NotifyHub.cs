using Microsoft.AspNet.SignalR;

namespace PerkPopUp
{
    public class NotifyHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}