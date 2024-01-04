using Hotel.DAL.Entities;

namespace Hotel.BLL.Dtos.Room
{
    public class RoomDto
    {
        public string RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
		public virtual RoomCategory Category { get; set; }
	}
}
