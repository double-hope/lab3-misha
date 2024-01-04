using Hotel.BLL.Dtos.Room;

namespace Hotel.BLL.Interfaces
{
    public interface IAvailabilityService
    {
        Task<IEnumerable<RoomDto>> GetAvailableRooms(DateTime startDate, DateTime endDate);
    }
}
