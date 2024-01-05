using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;

namespace Hotel.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelDataContext _context;

        private IRepository<Client> clientRepository;
        private IRepository<Reservation> reservationRepository;
        private IRepository<Room> roomRepository;
        private IRepository<RoomCategory> roomCategoryRepository;
        public UnitOfWork(HotelDataContext context)
        {
            _context = context;
        }
        public IRepository<Client> ClientRepository
        {
            get
            {
                if (clientRepository == null)
                {
                    clientRepository = new Repository<Client>(_context);
                }
                return clientRepository;
            }
        }

        public IRepository<Reservation> ReservationRepository
        {
            get
            {
                if (reservationRepository == null)
                {
                    reservationRepository = new Repository<Reservation>(_context);
                }
                return reservationRepository;
            }
        }

        public IRepository<Room> RoomRepository
        {
            get
            {
                if (roomRepository == null)
                {
                    roomRepository = new Repository<Room>(_context);
                }
                return roomRepository;
            }
        }

        public IRepository<RoomCategory> RoomCategoryRepository
        {
            get
            {
                if (roomCategoryRepository == null)
                {
                    roomCategoryRepository = new Repository<RoomCategory>(_context);
                }
                return roomCategoryRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
