using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using YahooFinanceAPI.Data;
using YahooFinanceAPI.Models;
using YahooFinanceAPI.Repository.IRepository;

namespace YahooFinanceAPI.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _db;
        public StockRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateStockAsync(Stock entity)
        {
            await _db.Stocks.AddAsync(entity);
            await SaveDataAsync();
        }        

        public async Task<List<Stock>> GetAllStocksAsync(Expression<Func<Stock, bool>> filter = null)
        {
            IQueryable<Stock> query = _db.Stocks;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Stock> GetStockAsync(Expression<Func<Stock, bool>> filter = null)
        {
            IQueryable<Stock> query = _db.Stocks;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        

        public async Task RemoveStockAsync(Stock entity)
        {
            _db.Stocks.Remove(entity);
            await SaveDataAsync();
        }

        public async Task SaveDataAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(Stock entity)
        {
            _db.Stocks.Update(entity);
            await SaveDataAsync();
        }

        public async Task CreateStockHistoricalDataAsync(StockHistoricalData entity)
        {
            await _db.StockHistoricalDatas.AddAsync(entity);
            await SaveDataAsync();
        }

        public async Task<List<StockHistoricalData>> GetAllStockHistoricalDataAsync(Expression<Func<StockHistoricalData, bool>> filter = null)
        {
            IQueryable<StockHistoricalData> query = _db.StockHistoricalDatas;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<StockHistoricalData> GetStockHistoricalDataAsync(Expression<Func<StockHistoricalData, bool>> filter = null)
        {
            IQueryable<StockHistoricalData> query = _db.StockHistoricalDatas;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Stock> GetStockWithHistoricalData(string stockSymbol, DateTime startDate, DateTime endDate)
        {
            return await _db.Stocks.Include(s => s.HistoricalData.Where(c => c.Date >= startDate && c.Date <= endDate)).FirstOrDefaultAsync(s => s.Symbol == stockSymbol);
        }

        
    }
}
