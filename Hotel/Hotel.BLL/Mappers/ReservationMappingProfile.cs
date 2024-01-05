using AutoMapper;
using Hotel.BLL.Dtos.Reservation;
using Hotel.DAL.Entities;

namespace Hotel.BLL.Mappers
{
	public class ReservationMappingProfile : Profile
	{
		public ReservationMappingProfile()
		{
			CreateMap<CreateReservationDto, Reservation>();
        }
	}
}
