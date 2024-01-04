using Hotel.Shared.Enums;

namespace Hotel.DAL.Entities
{
    public class RoomRental
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RentalStatus Status { get; set; }

        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
