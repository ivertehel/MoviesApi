using System;

namespace ApiApplication.Components.ReserveSeats
{
    public class ReserveSeatsCommandResult
    {
        public Guid ReservationId { get; set; }
        public int NumberOfSeats { get; set; }
        public int AuditoriumId { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}