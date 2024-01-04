using AutoMapper;
using Hotel.BLL.Dtos.Category;
using Hotel.DAL.Entities;

namespace Hotel.BLL.Mappers
{
	public class CategoryMappingProfile : Profile
	{
		public CategoryMappingProfile()
		{
			CreateMap<RoomCategory, CategoryDto>();
		}
	}
}
