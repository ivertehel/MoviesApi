using ApiApplication.Database.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IMoviesRepository
    {
        Task<MovieEntity> GetMovieByIdAsync(int id, CancellationToken cancel);
        Task<int> CreateMovieAsync(MovieEntity movieEntity, CancellationToken cancel);
    }
}