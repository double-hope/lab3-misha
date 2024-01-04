using Hotel.BLL.Dtos.Room;

namespace Hotel.BLL.Interfaces
{
	public interface IRoomService
	{
		Task<RoomDto> CreateRoom(string roomNumber, Guid categoryId, decimal pricePerNight);
	}
}
