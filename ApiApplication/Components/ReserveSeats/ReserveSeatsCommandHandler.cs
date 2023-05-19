using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Components.ReserveSeats
{
    public class ReserveSeatsCommandHandler : IRequestHandler<ReserveSeatsCommand, Result<ReserveSeatsCommandResult>>
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IShowtimesRepository _showtimesRepository;

        public ReserveSeatsCommandHandler(
            IAuditoriumsRepository auditoriumsRepository,
            IReservationsRepository reservationsRepository,
            IShowtimesRepository showtimesRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _reservationsRepository = reservationsRepository;
            _showtimesRepository = showtimesRepository;
        }

        public async Task<Result<ReserveSeatsCommandResult>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var seatNumberContiguous = request.Seats.GroupBy(s => s.Row).All(seats =>
            {
                var orderedSeats = seats.OrderBy(s => s.SeatNumber).Select(s => s.SeatNumber).ToList();
                return AreSeatNumbersContiguous(orderedSeats);
            });

            if (!seatNumberContiguous)
            { 
                return Result.Fail("All the seats, when doing a reservation, need to be contiguous");
            }

            var showtime = await _showtimesRepository.GetWithTicketsAndMovieByIdAsync(request.ShowtimeId, cancellationToken);
            if (showtime == null)
            {
                return Result.Fail("Showtime was not found");
            }

            var boughtSeats = showtime.Tickets.SelectMany(t => t.Seats).Where(busySeats => request.Seats.Any(f => f.Row == busySeats.Row && f.SeatNumber == busySeats.SeatNumber)).ToList();
            if (boughtSeats.Any())
            {
                return Result.Fail($"Seats {string.Join(',', boughtSeats.Select(s => $"({s.Row}, {s.SeatNumber})"))} are bought");
            }

            var auditorium = await _auditoriumsRepository.GetAsync(showtime.AuditoriumId, cancellationToken);

            var existingSeats = auditorium.Seats.Where(s => request.Seats.Any(r => r.Row == s.Row && r.SeatNumber == s.SeatNumber)).ToList();
            if (existingSeats.Count != request.Seats.Count())
            {
                return Result.Fail("Some seats are absent in this auditorium");
            }

            var reservations = await _reservationsRepository.GetReservationsAsync(showtime.Id, cancellationToken);
            var reservedSeats = reservations.Where(r => request.Seats.Any(s => s.Row == r.Row && s.SeatNumber == r.SeatNumber));
            if (reservedSeats.Any())
            {
                return Result.Fail($"Seats {string.Join(',', reservedSeats.Select(s => $"({s.Row}, {s.SeatNumber})"))} are reserved");
            }

            var reservationId = Guid.NewGuid();
            await _reservationsRepository.AddAsync(request.Seats.Select(r => new ReservationEntity
            {
                Id = reservationId,
                ReservedOn = DateTime.Now,
                Row = r.Row,
                SeatNumber = r.SeatNumber,
                Showtime = showtime
            }).ToList(), cancellationToken);

            return Result.Ok(new ReserveSeatsCommandResult
            {
                AuditoriumId = auditorium.Id,
                ImdbId = showtime.Movie.ImdbId,
                MovieId = showtime.Movie.Id,
                ReleaseDate = showtime.Movie.ReleaseDate,
                Title = showtime.Movie.Title,
                Stars = showtime.Movie.Stars,
                ReservationId = reservationId,
                NumberOfSeats = request.Seats.Count()
            });
        }

        private bool AreSeatNumbersContiguous(List<short> orderedSeats)
        {
            int previousSeatNumber = orderedSeats.First();

            for (int i = 1; i < orderedSeats.Count; i++)
            {
                var seat = orderedSeats[i];

                if (seat - previousSeatNumber > 1)
                {
                    return false;
                }

                previousSeatNumber = seat;
            }

            return true;
        }
    }
}