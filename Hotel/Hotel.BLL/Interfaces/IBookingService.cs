using Hotel.BLL.Dtos.Reservation;

namespace Hotel.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<bool> BookRoom(string clientEmail, Guid roomId, DateTime startDate, DateTime endDate);
        Task<bool> CancelBooking(Guid reservationId);
        Task<CompleteReservationDto> CompleteBooking(Guid reservationId);
    }
}
