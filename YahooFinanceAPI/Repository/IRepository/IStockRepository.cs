using System.Linq.Expressions;
using YahooFinanceAPI.Models;

namespace YahooFinanceAPI.Repository.IRepository
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync(Expression<Func<Stock, bool>> filter = null);
        Task<Stock> GetStockAsync(Expression<Func<Stock, bool>> filter = null);
        Task<Stock> GetStockWithHistoricalData(string stockSymbol, DateTime startDate, DateTime endDate);
        Task CreateStockAsync(Stock entity);
        Task UpdateStockAsync(Stock entity);
        Task RemoveStockAsync(Stock entity);
        Task SaveDataAsync();

        Task<List<StockHistoricalData>> GetAllStockHistoricalDataAsync(Expression<Func<StockHistoricalData, bool>> filter = null);
        Task<StockHistoricalData> GetStockHistoricalDataAsync(Expression<Func<StockHistoricalData, bool>> filter = null);       
        Task CreateStockHistoricalDataAsync(StockHistoricalData entity);

    }
}
