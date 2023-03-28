namespace YahooFinanceAPI.Models.YahooResponse
{
    public class YahooStockHistoricalDataResponse
    {
        public Chart chart { get; set; }
    }

    public class Chart
    {
        public HistoryResult[] result { get; set; }
        public object error { get; set; }
    }

    public class HistoryResult
    {
        public Meta meta { get; set; }
        public int[] timestamp { get; set; }
        public Indicators indicators { get; set; }
    }

    public class Meta
    {
        public string currency { get; set; }
        public string symbol { get; set; }
        public string exchangeName { get; set; }
        public string instrumentType { get; set; }
        public int firstTradeDate { get; set; }
        public int regularMarketTime { get; set; }
        public int gmtoffset { get; set; }
        public string timezone { get; set; }
        public string exchangeTimezoneName { get; set; }
        public float regularMarketPrice { get; set; }
        public float chartPreviousClose { get; set; }
        public int priceHint { get; set; }
        public Currenttradingperiod currentTradingPeriod { get; set; }
        public string dataGranularity { get; set; }
        public string range { get; set; }
        public string[] validRanges { get; set; }
    }

    public class Currenttradingperiod
    {
        public Pre pre { get; set; }
        public Regular regular { get; set; }
        public Post post { get; set; }
    }

    public class Pre
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Regular
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Post
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Indicators
    {
        public Quote[] quote { get; set; }
        public Adjclose[] adjclose { get; set; }
    }

    public class Quote
    {
        public decimal[] low { get; set; }
        public decimal[] high { get; set; }
        public decimal[] close { get; set; }
        public int[] volume { get; set; }
        public decimal[] open { get; set; }
    }

    public class Adjclose
    {
        public decimal[] adjclose { get; set; }
    }

}


