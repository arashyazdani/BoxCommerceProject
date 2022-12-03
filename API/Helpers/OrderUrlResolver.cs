using API.DTOs;
using AutoMapper;
using Domain.Entities.OrderAggregate;

namespace API.Helpers
{
    public class OrderUrlResolver : IValueResolver<Order,OrderToReturnDTO, string?>
    {
        private readonly IConfiguration _config;

        public OrderUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string? Resolve(Order source, OrderToReturnDTO destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
