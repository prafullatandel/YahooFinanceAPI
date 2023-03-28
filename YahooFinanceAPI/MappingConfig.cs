using AutoMapper;
using YahooFinanceAPI.Models;
using YahooFinanceAPI.Models.Dto;

namespace YahooFinanceAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Stock, StockDTO>();
            CreateMap<StockDTO, Stock>();

            CreateMap<Stock, StockDDLItemDTO>().ReverseMap();
            CreateMap<Stock, StockCreateDTO>().ReverseMap();
            CreateMap<StockHistoricalData, StockHistoricalDataDTO>().ReverseMap();
            CreateMap<StockHistoricalData, StockHistoricalDataCreateDTO>().ReverseMap();
        }
    }
}
