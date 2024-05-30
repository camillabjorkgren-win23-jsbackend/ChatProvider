using ChatProvider.Data.Context;
using ChatProvider.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("ChatDatabase"));

builder.Services.AddCors(x =>
{
    x.AddPolicy("ChatPolicy", builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("ChatPolicy");
app.MapHub<ChatHub>("/chathub");

app.Run();


