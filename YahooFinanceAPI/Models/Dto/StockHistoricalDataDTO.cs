using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace YahooFinanceAPI.Models.Dto
{
    public class StockHistoricalDataDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(20,8)")]
        public decimal Open { get; set; }
        [Column(TypeName = "decimal(20,8)")]
        public decimal Close { get; set; }
        [Column(TypeName = "decimal(20,8)")]
        public decimal Low { get; set; }
        [Column(TypeName = "decimal(20,8)")]
        public decimal High { get; set; }
        [Column(TypeName = "decimal(20,8)")]
        public decimal AdjustedClose { get; set; }
        public long Volume { get; set; }
    }
}
