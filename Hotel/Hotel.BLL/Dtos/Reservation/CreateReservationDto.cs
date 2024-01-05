namespace Hotel.BLL.Dtos.Reservation
{
    public class CreateReservationDto
    {
        public string ClientEmail { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
