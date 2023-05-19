using System;

namespace ApiApplication.Database.Entities
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }
        public int ShowtimeId { get; set; }
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public DateTime ReservedOn { get; set; }
        public ShowtimeEntity Showtime { get; set; }
    }
}