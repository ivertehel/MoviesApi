using ApiApplication.Database.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IReservationsRepository
    {
        Task<IEnumerable<ReservationEntity>> GetReservationsAsync(int showtimeId, CancellationToken cancel);
        Task AddAsync(IEnumerable<ReservationEntity> reservations, CancellationToken cancel);
        Task RemoveOutdatedReservationsAsync(CancellationToken cancel);
    }
}