using AutoMapper;
using AutoMapper.Execution;
using Stripe;

namespace API.Helpers
{
    public class PictureUrlResolver : IValueResolver<object, object, string?>
    {
        private readonly IConfiguration _config;

        public PictureUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string? Resolve(dynamic source, dynamic destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
