using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YahooFinanceAPI.Models
{
    public class StockHistoricalData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }

    }
}
