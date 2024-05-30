using ChatProvider.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatProvider.Data.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserConnection> Connections { get; set; }
}
