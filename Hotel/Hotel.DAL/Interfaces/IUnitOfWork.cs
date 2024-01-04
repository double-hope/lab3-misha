using Hotel.DAL.Entities;

namespace Hotel.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Client> ClientRepository { get; }
        IRepository<Reservation> ReservationRepository { get; }
        IRepository<Room> RoomRepository { get; }
        IRepository<RoomCategory> RoomCategoryRepository { get; }
        IRepository<RoomRental> RoomRentalRepository { get; }
        Task SaveAsync();
    }
}
