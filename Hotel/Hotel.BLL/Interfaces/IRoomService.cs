using Hotel.BLL.Dtos.Room;

namespace Hotel.BLL.Interfaces
{
	public interface IRoomService
	{
		Task<RoomDto> CreateRoom(CreateRoomDto roomDto);
	}
}
