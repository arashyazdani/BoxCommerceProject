using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    // Maybe it will be better to not using this line because by changing the name of our controllers our API will be changed and it's not best practice. But, I prefer to don't change my controllers in my refactoring.
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}