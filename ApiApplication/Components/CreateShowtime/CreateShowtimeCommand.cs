using FluentResults;
using MediatR;
using System;

namespace ApiApplication.Components.CreateShowtime
{
    public class CreateShowtimeCommand : IRequest<Result<CreateShowtimeCommandResult>>
    {
        public int MovieId { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}