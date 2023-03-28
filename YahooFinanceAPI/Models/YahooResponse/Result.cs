namespace YahooFinanceAPI.Models.YahooResponse
{
    public class Result
    {
        public string? symbol { get; set; }
        public string? shortName { get; set; }
        public string? longName { get; set; }
        public string? quoteType { get; set; }
        public string? currency { get; set; }
        public string? fullExchangeName { get; set; }
        public string? regularMarketPrice { get; set; }
        public string? regularMarketOpen { get; set; }
        public string? regularMarketPreviousClose { get; set; }
        public string? regularMarketDayLow { get; set; }
        public string? regularMarketDayHigh { get; set; }
        public string? regularMarketVolume { get; set; }
        public string? regularMarketChange { get; set; }
        public string? regularMarketChangePercent { get; set; }
    }
}
