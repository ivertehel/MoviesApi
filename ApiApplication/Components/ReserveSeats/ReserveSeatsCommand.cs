using FluentResults;
using MediatR;
using System.Collections.Generic;

namespace ApiApplication.Components.ReserveSeats
{
    public class ReserveSeatsCommand : IRequest<Result<ReserveSeatsCommandResult>>
    {
        public int ShowtimeId { get; set; }
        public IEnumerable<SeatRequestModel> Seats { get; set; }
    }

    public class SeatRequestModel
    {
        public short Row { get; set; }
        public short SeatNumber { get; set; }
    }
}