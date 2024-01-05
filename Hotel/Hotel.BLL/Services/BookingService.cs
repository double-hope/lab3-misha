using AutoMapper;
using Hotel.BLL.Dtos.Reservation;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;
using Hotel.Shared.Enums;

namespace Hotel.BLL.Services
{
    public class BookingService : BaseService, IBookingService
    {
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<bool> BookRoom(CreateReservationDto reservationDto)
        {
            var client = await _unitOfWork.ClientRepository.FirstOrDefaultAsync(c => c.Email.Equals(reservationDto.ClientEmail));

            if(client == null)
            {
                throw new KeyNotFoundException("User with this email does not found");
            }

            var isRoomAvailable = await IsRoomAvailable(reservationDto.RoomId, reservationDto.StartDate, reservationDto.EndDate);

            if (isRoomAvailable)
            {
                var reservation = _mapper.Map<Reservation>(reservationDto);
                reservation.ClientId = client.Id;
                reservation.Status = ReservationStatus.Active;
                
                await _unitOfWork.ReservationRepository.AddAsync(reservation);

                await _unitOfWork.SaveAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> CancelBooking(Guid reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.FirstOrDefaultAsync(r => r.Id.Equals(reservationId));

            if (reservation == null)
            {
                throw new KeyNotFoundException("Reservation with this id does not found");
            }

            if (reservation.Status == ReservationStatus.Canceled)
            {
                throw new InvalidOperationException("You cannot cancel already cancelled reservation");
            }

            if (reservation.Status == ReservationStatus.Completed)
            {
                throw new InvalidOperationException("You cannot cancel completed reservation");
            }

            reservation.Status = ReservationStatus.Canceled;
            _unitOfWork.ReservationRepository.Update(reservation);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<CompleteReservationDto> CompleteBooking(Guid reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.FirstOrDefaultAsync(r => r.Id.Equals(reservationId));

            if (reservation == null)
            {
                throw new KeyNotFoundException("Reservation with this id does not found");
            }

            if (reservation.Status == ReservationStatus.Canceled)
            {
                throw new InvalidOperationException("You cannot cancel already cancelled reservation");
            }

            if (reservation.Status == ReservationStatus.Completed)
            {
                throw new InvalidOperationException("You cannot cancel completed reservation");
            }

            var room = await _unitOfWork.RoomRepository.FirstOrDefaultAsync(r => r.Id.Equals(reservation.RoomId));
            var category = await _unitOfWork.RoomCategoryRepository.FirstOrDefaultAsync(r => r.Id.Equals(room.CategoryId));

            var completeReservationDto = new CompleteReservationDto()
            {
                AmountOfMoney = room.PricePerNight * (decimal)category.PriceCoefficient,
            };

            reservation.Status = ReservationStatus.Completed;
            _unitOfWork.ReservationRepository.Update(reservation);

            await _unitOfWork.SaveAsync();

            return completeReservationDto;
        }


        private async Task<bool> IsRoomAvailable(Guid roomId, DateTime startDate, DateTime endDate)
        {

            var room = await _unitOfWork.RoomRepository.FirstOrDefaultAsync(r => r.Id.Equals(roomId));

            if(room == null)
            {
                throw new KeyNotFoundException("Room with this id does not found");
            }

            var reservations = await _unitOfWork.ReservationRepository
            .GetAllAsync(r =>
                r.RoomId == room.Id &&
                ((startDate >= r.StartDate && startDate <= r.EndDate && r.Status == ReservationStatus.Active) ||
                 (endDate >= r.StartDate && endDate <= r.EndDate && r.Status == ReservationStatus.Active) ||
                 (startDate <= r.StartDate && endDate >= r.EndDate && r.Status == ReservationStatus.Active)));

            return !reservations.Any();
        }
    }
}
