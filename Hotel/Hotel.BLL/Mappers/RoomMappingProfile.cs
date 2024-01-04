using AutoMapper;
using Hotel.BLL.Dtos.Room;
using Hotel.DAL.Entities;

namespace Hotel.BLL.Mappers
{
	public class RoomMappingProfile : Profile
	{
		public RoomMappingProfile()
		{
			CreateMap<Room, RoomDto>();
		}
	}
}
