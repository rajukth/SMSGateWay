using Microsoft.AspNetCore.SignalR;

namespace SMSGateway.Hubs;

public class SmsHub: Hub
{
    public async Task NotifyStatusChange(int messageId, string status,DateTime sentAt, string response)
    {
       await Clients.All.SendAsync("ReceiveStatusUpdate", messageId, status,sentAt.ToString("MM/dd/yyyy hh:mm tt"), response);
    }
}