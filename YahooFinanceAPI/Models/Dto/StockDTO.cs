using System.ComponentModel.DataAnnotations;

namespace YahooFinanceAPI.Models.Dto
{
    public class StockDTO
    {
        public int StockId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public ICollection<StockHistoricalDataDTO>? HistoricalData { get; set; }
    }
}
