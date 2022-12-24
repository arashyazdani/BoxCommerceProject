using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    //// It will be better to not using this line because by changing the name of our controllers our API will be changed and it's not best practice.
    //[Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}