using Microsoft.EntityFrameworkCore;
using YahooFinanceAPI.Models;

namespace YahooFinanceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockHistoricalData> StockHistoricalDatas { get; set; }

    }
}
