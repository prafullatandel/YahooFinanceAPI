using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YahooFinanceAPI.Models
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }        
        public string Symbol { get; set; }
        public string Name { get; set; }
        public ICollection<StockHistoricalData>? HistoricalData { get; set; }
    }
}
