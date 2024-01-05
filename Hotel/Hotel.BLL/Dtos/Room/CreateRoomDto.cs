namespace Hotel.BLL.Dtos.Room
{
    public class CreateRoomDto
    {
        public string RoomNumber { get; set; }
        public Guid CategoryId { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
