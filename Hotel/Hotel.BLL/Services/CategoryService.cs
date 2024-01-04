using AutoMapper;
using Hotel.BLL.Dtos.Category;
using Hotel.BLL.Interfaces;
using Hotel.DAL.Entities;
using Hotel.DAL.Interfaces;

namespace Hotel.BLL.Services
{
	public class CategoryService : BaseService, ICategoryService
	{
		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

		public async Task<IEnumerable<RoomCategory>> GetAllCategories()
		{
			var categories = await _unitOfWork.RoomCategoryRepository.GetAllAsync();

			return categories;
		}

		public async Task<RoomCategory> CreateCategory(string categoryName, float priceCoefficient)
		{
			var category = new RoomCategory
			{
				Id = Guid.NewGuid(),
				Name = categoryName,
				PriceCoefficient = priceCoefficient
			};

			await _unitOfWork.RoomCategoryRepository.AddAsync(category);
			await _unitOfWork.SaveAsync();

			return category;
		}
	}
}
