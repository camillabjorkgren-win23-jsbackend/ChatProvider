using ChatProvider.ApiKey;
using ChatProvider.Data.Context;
using ChatProvider.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatProvider.Hubs;

[UseApiKey]
public class ChatHub(DataContext context) : Hub
{
    private readonly DataContext _context = context;

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Typing(string userName)
    {
        await Clients.Others.SendAsync("UserTyping", userName);
    }
    public async Task SendMessageToAll(string userName, string message, DateTime dateTime)
    {
        try
        {
            var newUserConnection = new UserConnection
            {
                UserName = userName,
                ConnectionId = Context.ConnectionId // Tilldela värdet från Context.ConnectionId
            };

            // Lägg till den nya posten i databasen
            _context.Connections.Add(newUserConnection);
            await _context.SaveChangesAsync();

            var uc = await _context.Connections.FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);
            if (uc != null)
            {
                await Clients.All.SendAsync("ReceiveMessage", userName, message, dateTime);
            }
            Console.WriteLine("Message sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
