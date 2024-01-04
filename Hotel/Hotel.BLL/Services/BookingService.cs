using AutoMapper;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;
using Hotel.Shared.Enums;

namespace Hotel.BLL.Services
{
    public class BookingService : BaseService, IBookingService
    {
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<bool> BookRoom(Guid clientId, Guid roomId, DateTime startDate, DateTime endDate)
        {
            var isRoomAvailable = await IsRoomAvailable(roomId, startDate, endDate);

            if (isRoomAvailable)
            {
                var reservation = new Reservation
                {
                    ClientId = clientId,
                    RoomId = roomId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = ReservationStatus.Active
                };
                
                await _unitOfWork.ReservationRepository.AddAsync(reservation);

                await _unitOfWork.SaveAsync();

                return true;
            }

            return false;
        }

        private async Task<bool> IsRoomAvailable(Guid roomId, DateTime startDate, DateTime endDate)
        {
            var reservations = await _unitOfWork.ReservationRepository
            .GetAllAsync(r =>
                r.RoomId == roomId &&
                ((startDate >= r.StartDate && startDate <= r.EndDate) ||
                 (endDate >= r.StartDate && endDate <= r.EndDate) ||
                 (startDate <= r.StartDate && endDate >= r.EndDate)));

            return !reservations.Any();
        }
    }
}
