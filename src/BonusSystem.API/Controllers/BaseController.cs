using Microsoft.AspNetCore.Mvc;

namespace BonusSystem.Controllers
{
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public class BaseController : ControllerBase
    {
    }
}
