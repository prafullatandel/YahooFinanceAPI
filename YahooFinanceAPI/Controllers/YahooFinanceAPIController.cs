using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YahooFinanceAPI.Migrations;
using YahooFinanceAPI.Models;
using YahooFinanceAPI.Models.Dto;
using YahooFinanceAPI.Models.YahooResponse;
using YahooFinanceAPI.Repository.IRepository;

namespace YahooFinanceAPI.Controllers
{
    [Authorize]
    [Route("api/YahooFinanceStocksAPI")]
    [ApiController]
    public class YahooFinanceAPIController : ControllerBase
    {
        private readonly IStockRepository _dbStocks;
        private readonly IMapper _mapper;
        private readonly ILogger<YahooFinanceAPIController> _logger;

        public YahooFinanceAPIController(IStockRepository stockRepository, IMapper mapper, ILogger<YahooFinanceAPIController> logger)
        {
            _dbStocks = stockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetCurrentStockDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StockDTO>> GetCurrentStockDetailsAsyc(String stockSymbol)
        {
            if(stockSymbol == null) throw new ArgumentNullException(nameof(stockSymbol));

            try
            {
                using (var client = new HttpClient())
                {
                    var dataUrl = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=AU&corsDomain=finance.yahoo.com&symbols={stockSymbol}";
                    var dataResponse = await client.GetAsync(dataUrl);
                    if (!dataResponse.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    dataResponse.EnsureSuccessStatusCode();

                    var responseContent = await dataResponse.Content.ReadAsStringAsync();

                    var responseStockData = JsonConvert.DeserializeObject<YahooResponseStockDetails>(responseContent);
                    var stockDetails = responseStockData?.quoteResponse?.result?.Length > 0 ? responseStockData?.quoteResponse?.result[0] : null;

                    if(stockDetails == null)
                    {
                        _logger.LogError($"{stockSymbol} Not found at API end point.");
                        return NotFound();
                    }

                    var stockModel = new StockDetailsDTO();
                    stockModel.Symbol = stockDetails?.symbol;
                    stockModel.ShortName = stockDetails?.shortName;
                    stockModel.LongName = stockDetails?.longName;
                    stockModel.QuoteType = stockDetails?.quoteType;
                    stockModel.Currency =  stockDetails?.currency;
                    stockModel.RegularMarketPrice = stockDetails?.regularMarketPrice;
                    stockModel.RegularMarketOpen = stockDetails?.regularMarketOpen;
                    stockModel.RegularMarketPreviousClose = stockDetails?.regularMarketPreviousClose;
                    stockModel.RegularMarketDayLow = stockDetails?.regularMarketDayLow;
                    stockModel.RegularMarketDayHigh = stockDetails?.regularMarketDayHigh;
                    stockModel.RegularMarketVolume = stockDetails?.regularMarketVolume;
                    stockModel.RegularMarketChange = stockDetails?.regularMarketChange;
                    stockModel.RegularMarketChangePercent = stockDetails?.regularMarketChangePercent;

                    _logger.LogInformation($" Found {stockSymbol} details. {stockModel.ToString()}");

                    return Ok(stockModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }

        [HttpGet("GetStockHistoricalData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Object>> GetStockHistoricalDataAsync(string stockSymbol)
        {
            try
            {
                var startDate = DateTime.Today.AddYears(-1);
                var endDate = DateTime.Today;

                var latestStockRecord = await GetLatestHistoricalData(stockSymbol);
                if (latestStockRecord != null && latestStockRecord.Date == DateTime.Now.Date) 
                {
                    return _mapper.Map<StockDTO>(await _dbStocks.GetStockWithHistoricalData(stockSymbol, startDate, endDate));
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var rageValue = await GetDurationString(stockSymbol);
                        var dataUrl = $"https://query1.finance.yahoo.com/v8/finance/chart/{stockSymbol}?range={rageValue}&interval=1d&region=AU&lang=en-AU";
                        var dataResponse = await client.GetAsync(dataUrl);
                        if (!dataResponse.IsSuccessStatusCode)
                        {
                            return NotFound();
                        }
                        dataResponse.EnsureSuccessStatusCode();
                        var responseContent = await dataResponse.Content.ReadAsStringAsync();

                        var responseStockData = JsonConvert.DeserializeObject<YahooStockHistoricalDataResponse>(responseContent);

                        if (responseStockData != null && responseStockData.chart != null && responseStockData.chart.result != null)
                        {

                            foreach (var historicalData in responseStockData.chart.result)
                            {
                                var timestamps = historicalData.timestamp;
                                var quotes = historicalData.indicators.quote[0];
                                var adjustedQuotes = historicalData.indicators.adjclose[0];
                                var historicalDataToReturn = new List<StockHistoricalData>();
                                for (int i = 0; i < timestamps.Length; i++)
                                {

                                    var hdData = new StockHistoricalData();
                                    hdData.Date = DateTimeOffset.FromUnixTimeSeconds(timestamps[i]).LocalDateTime.Date;
                                    hdData.Open = quotes.open[i];
                                    hdData.High = quotes.high[i];
                                    hdData.Low = quotes.low[i];
                                    hdData.Close = quotes.close[i];
                                    hdData.Volume = quotes.volume[i];
                                    hdData.AdjustedClose = adjustedQuotes.adjclose[i];

                                    historicalDataToReturn.Add(hdData);
                                }
                                var StockObject = new Stock
                                {
                                    Symbol = stockSymbol,
                                    Name = stockSymbol,
                                    HistoricalData = historicalDataToReturn
                                };
                                //return Ok(StockObject);
                                var stockModel = await AddStockHistoricalData(stockSymbol, historicalDataToReturn, startDate, endDate);
                                if (stockModel == null)
                                {
                                    NotFound();
                                }
                                return Ok(stockModel);
                            }
                        }
                        else
                        {
                            return NotFound();
                        }

                        return Ok(responseStockData);

                    }
                }


               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }


        [HttpGet("GetStockList")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Stock>>> GetStockList()
        {
            var stockList = await _dbStocks.GetAllStocksAsync();
            return Ok(_mapper.Map<List<Stock>,List<StockDDLItemDTO>>(stockList));
        }

        [HttpGet("{id:int}", Name = "GetStockById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StockDTO>> GetStockById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var stock = await _dbStocks.GetStockAsync(s => s.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<StockDTO>(stock));
        }

        [HttpPost]
        public async Task<ActionResult<StockDTO>> CreateStock([FromBody] StockCreateDTO createDTO)
        {
            if (await _dbStocks.GetStockAsync(s => s.Symbol.ToUpper() == createDTO.Symbol.ToUpper()) != null)
            {
                ModelState.AddModelError("Symbol", "Stock Symbol already exists.");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Stock model = _mapper.Map<Stock>(createDTO);
            await _dbStocks.CreateStockAsync(model);
            return CreatedAtRoute("GetStockById", new { id = model.StockId }, model);
        }

        private async Task<StockDTO> AddStockHistoricalData(string stockSymbol, List<StockHistoricalData> historicalData, DateTime startDate, DateTime endDate)
        {
          

            List<StockHistoricalData> hdModel = new List<StockHistoricalData>();
            if (stockSymbol == null)
            {
                return null;
            }
            var stock = await _dbStocks.GetStockAsync(s => s.Symbol == stockSymbol);
            if(stock != null)
            {
                foreach (var hd in historicalData)
                {
                    //StockHistoricalData model = _mapper.Map<StockHistoricalData>(hd);
                    hd.StockId = stock.StockId;
                    if (await _dbStocks.GetStockHistoricalDataAsync(shd => shd.StockId == stock.StockId && shd.Date == hd.Date) == null)
                    {
                        await _dbStocks.CreateStockHistoricalDataAsync(hd);
                    }
                    //stock.HistoricalData?.Add(hd);                    
                }
                return _mapper.Map<StockDTO>(await _dbStocks.GetStockWithHistoricalData(stockSymbol, startDate, endDate));
            }
            return null;
        }

        private async Task<String> GetDurationString(string stockSymbol)
        {
            var latest = await GetLatestHistoricalData(stockSymbol);

            if(latest != null)
            {
                var latestRecordDifferenceInDays = (DateTime.Now - latest.Date).Days;

                if (latestRecordDifferenceInDays == 0)
                {
                    return "";
                }
                else if (latestRecordDifferenceInDays > 0 && latestRecordDifferenceInDays <= 1)
                {
                    return "1d";
                }
                else if (latestRecordDifferenceInDays > 1 && latestRecordDifferenceInDays <= 5)
                {
                    return "5d";
                }
                else if (latestRecordDifferenceInDays > 5 && latestRecordDifferenceInDays <= 30)
                {
                    return "1mo";
                }
                else if (latestRecordDifferenceInDays > 30 && latestRecordDifferenceInDays <= 90)
                {
                    return "3mo";
                }
                else if (latestRecordDifferenceInDays > 90 && latestRecordDifferenceInDays <= 180)
                {
                    return "6mo";
                }
                else
                {
                    return "1y";
                }
            }

            return "1y";           
        }

        private async Task<StockHistoricalData> GetLatestHistoricalData(string stockSymbol)
        {
            var stock = await _dbStocks.GetStockAsync(s => s.Symbol == stockSymbol);
            if (stock != null)
            {
                var result = await _dbStocks.GetAllStockHistoricalDataAsync(shd => shd.StockId == stock.StockId);
                if (result != null)
                {
                    var sortedResult = result.OrderBy(r => r.Date);

                    var latest = sortedResult.LastOrDefault();

                    return latest;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

    }

    
}
