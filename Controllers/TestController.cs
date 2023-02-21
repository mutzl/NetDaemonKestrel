using Microsoft.AspNetCore.Mvc;

namespace Mutzl.NetDaemon.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetHelloWorld()
    {
        return Ok("Hello from Controller!");
    } 
}
