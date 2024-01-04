using Hotel.DAL.Entities;

namespace Hotel.BLL.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<RoomCategory>> GetAllCategories();
		Task<RoomCategory> CreateCategory(string categoryName, float priceCoefficient);
	}
}
