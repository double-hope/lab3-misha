﻿namespace Hotel.DAL.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
