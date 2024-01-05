using Hotel.BLL.Dtos.Reservation;

namespace Hotel.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<bool> BookRoom(CreateReservationDto reservationDto);
        Task<bool> CancelBooking(Guid reservationId);
        Task<CompleteReservationDto> CompleteBooking(Guid reservationId);
    }
}
