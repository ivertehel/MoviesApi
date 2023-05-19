﻿using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAuditoriumAsync(AuditoriumEntity auditoriumEntity, CancellationToken cancellationToken)
        {
            var auditorium = await _context.Auditoriums.AddAsync(auditoriumEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return auditorium.Entity.Id;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }
    }
}
