using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly CinemaContext _context;

        public ReservationsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReservationEntity>> GetReservationsAsync(int showtimeId, CancellationToken cancel)
        {
            return await _context.Reservations.Where(x => x.ShowtimeId == showtimeId).ToListAsync(cancel);
        }

        public async Task AddAsync(IEnumerable<ReservationEntity> reservations, CancellationToken cancel)
        {
            await _context.Reservations.AddRangeAsync(reservations);
            await _context.SaveChangesAsync(cancel);
        }

        public async Task RemoveOutdatedReservationsAsync(CancellationToken cancel)
        {
            var outdatedReservations = await _context.Reservations.Where(r => r.ReservedOn < DateTime.Now.AddMinutes(-10)).ToListAsync();
            if (outdatedReservations.Any())
            {
                _context.Reservations.RemoveRange(outdatedReservations);
            }

            await _context.SaveChangesAsync(cancel);
        }
    }
}