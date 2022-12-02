using API.DTOs;
using AutoMapper;
using AutoMapper.Execution;
using Domain.Entities;

namespace API.Helpers
{
    public class VehicleImageUrlResolver : IValueResolver<Vehicle, VehicleToReturnDto,string?>
    {
        private readonly IConfiguration _config;

        public VehicleImageUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string? Resolve(Vehicle source, VehicleToReturnDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
