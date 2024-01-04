using AutoMapper;
using Hotel.BLL.Dtos.Room;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Interfaces;

namespace Hotel.BLL.Services
{
    public class AvailabilityService : BaseService, IAvailabilityService
    {
        public AvailabilityService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<IEnumerable<RoomDto>> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var reservedRoomIds = await GetReservedRoomIds(startDate, endDate);

            var availableRooms = await _unitOfWork.RoomRepository
                .GetAllAsync(room => !reservedRoomIds.Contains(room.Id));

            var availableRoomsDto = _mapper.Map<IEnumerable<RoomDto>>(availableRooms);

            return availableRoomsDto;
        }

        private async Task<IEnumerable<Guid>> GetReservedRoomIds(DateTime startDate, DateTime endDate)
        {
            var reservations = await _unitOfWork.ReservationRepository
                .GetAllAsync(r =>
                    (startDate >= r.StartDate && startDate <= r.EndDate) ||
                    (endDate >= r.StartDate && endDate <= r.EndDate) ||
                    (startDate <= r.StartDate && endDate >= r.EndDate));

            return reservations.Select(r => r.RoomId).ToList();
        }
    }
}
