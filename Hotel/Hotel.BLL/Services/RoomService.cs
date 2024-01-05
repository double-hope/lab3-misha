using AutoMapper;
using Hotel.BLL.Dtos.Room;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;

namespace Hotel.BLL.Services
{
	public class RoomService : BaseService, IRoomService
	{
		public RoomService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

		public async Task<RoomDto> CreateRoom(CreateRoomDto roomDto)
		{

			var category = await _unitOfWork.RoomCategoryRepository.FirstOrDefaultAsync(c => c.Id == roomDto.CategoryId);

			if(category == null)
			{
				throw new KeyNotFoundException("Category was not found");
			}

			var room = _mapper.Map<Room>(roomDto);

			await _unitOfWork.RoomRepository.AddAsync(room);
			await _unitOfWork.SaveAsync();

			return _mapper.Map<RoomDto>(room);
		}
	}
}
