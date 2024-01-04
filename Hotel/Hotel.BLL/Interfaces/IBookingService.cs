namespace Hotel.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<bool> BookRoom(Guid clientId, Guid roomId, DateTime startDate, DateTime endDate);
    }
}
