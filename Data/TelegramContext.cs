using ClientTelegram.Models;
using Microsoft.EntityFrameworkCore;
namespace ClientTelegram.Data
{
    class TelegramContext : DbContext
    {
        public DbSet<Users>? Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=telegram;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
