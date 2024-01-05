namespace Hotel.DAL.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public Guid CategoryId { get; set; }
        public virtual RoomCategory Category { get; set; }
        public decimal PricePerNight { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
