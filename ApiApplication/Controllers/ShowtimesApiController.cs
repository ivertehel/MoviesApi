using ApiApplication.Components.CreateShowtime;
using ApiApplication.Components.ReserveSeats;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [ApiController]
    [Route("showtimes")]
    public class ShowtimesApiController : BaseController
    {
        private readonly IMediator _mediator;

        public ShowtimesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShowtimeCommand request)
        {
            var result = await _mediator.Send(request);
            return ReturnResponse(result);
        }

        [HttpPost("{id}/reserve-seats")]
        public async Task<IActionResult> ReserveSeats([FromRoute] int id, [FromBody] IEnumerable<SeatRequestModel> seats)
        {
            var result = await _mediator.Send(new ReserveSeatsCommand
            {
                Seats = seats,
                ShowtimeId = id
            });

            return ReturnResponse(result);
        }
    }
}