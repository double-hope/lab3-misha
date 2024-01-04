using Hotel.Shared.Enums;

namespace Hotel.DAL.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }

        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }

        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
