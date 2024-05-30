using ChatProvider.Data.Context;
using ChatProvider.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatProvider.Hubs;

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
    //public async Task JoinChat(UserConnection uc)
    //{
    //    await Clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToString("HH:mm"), $"{uc.UserName} has joined the chat");
    //}

    //public async Task JoinSpecificChatRoom(UserConnection uc)
    //{
    //    uc.ConnectionId = Context.ConnectionId;
    //    await Groups.AddToGroupAsync(uc.ConnectionId, uc.ChatRoom);

    //    _context.Add(uc);
    //    await _context.SaveChangesAsync();

    //    await Clients.Groups(uc.ChatRoom).SendAsync("ReceiveMessage", DateTime.Now.ToString("HH:mm"), $"{uc.UserName} has joined the chat");
    //}

    //public async Task SendMessage(string message)
    //{
    //    var uc = await _context.Connections.FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);
    //    if(uc != null)
    //    {
    //        await Clients.Groups(uc.ChatRoom).SendAsync("ReceiveMessage", DateTime.Now.ToString("HH:mm"), uc.UserName, message);

    //    }

    //}

    //public async Task StartTyping(string userName, string chatRoom)
    //{
    //    await Clients.Group(chatRoom).SendAsync("UserTyping", userName);
    //}



}
