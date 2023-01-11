using Microsoft.EntityFrameworkCore;

namespace Parser
{
    public class CurrencyExchangeContext : DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-5OOCU00\\SQLEXPRESS01;Database=ExchangeRates;Trusted_Connection=True;");
        }
    }
}
