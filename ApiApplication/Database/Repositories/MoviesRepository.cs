using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly CinemaContext _context;

        public MoviesRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<MovieEntity> GetMovieByIdAsync(int id, CancellationToken cancel)
        {
            return await _context.Movies.FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public async Task<int> CreateMovieAsync(MovieEntity movieEntity, CancellationToken cancel)
        {
            var movie = await _context.Movies.AddAsync(movieEntity, cancel);
            await _context.SaveChangesAsync(cancel);
            return movie.Entity.Id;
        }
    }
}