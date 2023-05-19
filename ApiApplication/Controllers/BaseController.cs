using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiApplication.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult ReturnResponse<T>(Result<T> result)
        {
            if (!result.IsFailed)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors.First().Message);
        }

        protected IActionResult ReturnResponse(Result result)
        {
            if (!result.IsFailed)
            {
                return Ok();
            }

            return BadRequest(result.Errors.First().Message);
        }
    }
}