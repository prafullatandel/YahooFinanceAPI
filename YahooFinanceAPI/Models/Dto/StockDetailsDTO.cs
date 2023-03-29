namespace YahooFinanceAPI.Models.Dto
{
    public class StockDetailsDTO
    {
        public string? Symbol { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public string? QuoteType { get; set; }
        public string? Currency { get; set; }
        public string? RegularMarketPrice { get; set; }
        public string? RegularMarketOpen { get; set; }
        public string? RegularMarketPreviousClose { get; set; }
        public string? RegularMarketDayLow { get; set; }
        public string? RegularMarketDayHigh { get; set; }
        public string? RegularMarketVolume { get; set; }
        public string? RegularMarketChange { get; set; }
        public string? RegularMarketChangePercent { get; set; }
        public override string ToString()
        {
            return $"Symbol: {Symbol} LongName : {LongName} Price : {RegularMarketPrice}";
        }

    }
}
