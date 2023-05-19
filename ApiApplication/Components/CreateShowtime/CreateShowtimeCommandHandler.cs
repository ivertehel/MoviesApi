using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using AutoMapper;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Components.CreateShowtime
{
    public class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, Result<CreateShowtimeCommandResult>>
    {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IMoviesRepository _moviesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IMapper _mapper;

        public CreateShowtimeCommandHandler(
            IShowtimesRepository showtimesRepository, 
            IMoviesRepository moviesRepository, 
            IAuditoriumsRepository auditoriumsRepository,
            IMapper mapper)
        {
            _showtimesRepository = showtimesRepository;
            _moviesRepository = moviesRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _mapper = mapper;
        }

        public async Task<Result<CreateShowtimeCommandResult>> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await _moviesRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);
            if (movie == null)
            {
                return Result.Fail("Movie not found");
            }

            var auditorium = await _auditoriumsRepository.GetAsync(request.AuditoriumId, cancellationToken);
            if (auditorium == null)
            {
                return Result.Fail("Auditorium not found");
            }

            await _showtimesRepository.CreateShowtime(new ShowtimeEntity
            {
                AuditoriumId = request.AuditoriumId,
                Movie = movie,
                SessionDate = request.SessionDate
            }, cancellationToken);

            return _mapper.Map<CreateShowtimeCommandResult>(movie);
        }
    }
}