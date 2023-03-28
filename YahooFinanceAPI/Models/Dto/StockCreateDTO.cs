using System.ComponentModel.DataAnnotations;

namespace YahooFinanceAPI.Models.Dto
{
    public class StockCreateDTO
    {
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
