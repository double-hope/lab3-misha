using Hotel.BLL.Dtos.Client;
using Hotel.BLL.Interfaces;
using Hotel.BLL.Services;
using Hotel.DAL;
using Hotel.DAL.DbSeeder;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Hotel.BLL.Mappers;
using Hotel.DAL.Entities;

namespace Hotel.ConsolePL
{
	public class Program
	{
		private ClientDto _client;
		public ServiceProvider ConfigureServices()
		{
			var serviceProvider = new ServiceCollection()
				.AddAutoMapper(conf =>
				{
					conf.AddProfiles(
						new List<Profile>()
						{
							new ClientMappingProfile(),
							new RoomMappingProfile(),
							new CategoryMappingProfile(),
						});
				})
				.AddDbContext<HotelDataContext>()
				.AddScoped<IDbSeeder, DbSeeder>()
				.AddScoped<IUnitOfWork, UnitOfWork>()
				.AddScoped<IAuthService, AuthService>()
				.AddScoped<IAvailabilityService, AvailabilityService>()
				.AddScoped<IBookingService, BookingService>()
				.AddScoped<IRoomService, RoomService>()
				.AddScoped<ICategoryService, CategoryService>()
				.BuildServiceProvider();

			using (var scope = serviceProvider.CreateScope())
			{
				var initializer = scope.ServiceProvider.GetRequiredService<IDbSeeder>();

				initializer.Seed();
			}

			return serviceProvider;
		}
		public async static Task Main(string[] args)
		{
			var app = new Program();
			var serviceProvider = app.ConfigureServices();

			await app.Run(serviceProvider);
		}

		public async Task Run(ServiceProvider serviceProvider)
		{
			while (true)
			{
				Console.WriteLine("");
				Console.WriteLine("----- Application -----");
				Console.WriteLine("1. Register");
				Console.WriteLine("2. Login");
				Console.WriteLine("3. Logout");
				Console.WriteLine("4. Check availability");
				Console.WriteLine("5. Book room");
				Console.WriteLine("6. Create room");
				Console.WriteLine("7. Create category");
				Console.WriteLine("To exit (/stop)");
				Console.WriteLine("");

				Console.Write("Enter command: ");
				string userInput = Console.ReadLine();
				Console.WriteLine("");

				if (userInput.Equals("/stop", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}

				await ProcessCommand(userInput, serviceProvider);
			}
		}

		private async Task ProcessCommand(string command, ServiceProvider serviceProvider)
		{
			switch (command.ToLower())
			{
				case "1":
					await Register(serviceProvider);
					break;
				case "2":
					await Login(serviceProvider);
					break;
				case "3":
					LogOut();
					break;
				case "4":
					await CheckAvailability(serviceProvider);
					break;
				case "5":
					await BookRoom(serviceProvider);
					break;
				case "6":
					await CreateRoom(serviceProvider);
					break;
				case "7":
					await CreateCategory(serviceProvider);
					break;
				default:
					Console.WriteLine("Wrong");
					break;
			}
		}
		private async Task Register(ServiceProvider serviceProvider)
		{
			Console.WriteLine("Registering:");
			var clientDto = new ClientRegisterDto();
			Console.Write("Enter first name: ");
			clientDto.FirstName = Console.ReadLine() ?? throw new NullReferenceException();
			Console.Write("Enter second nameе: ");
			clientDto.LastName = Console.ReadLine() ?? throw new NullReferenceException();
			Console.Write("Enter email: ");
			clientDto.Email = Console.ReadLine() ?? throw new NullReferenceException();
			Console.Write("Enter password: ");
			clientDto.Password = Console.ReadLine() ?? throw new NullReferenceException();
			using (var scope = serviceProvider.CreateScope())
			{
				var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
				try
				{
					await authService.Register(clientDto);

					Console.WriteLine("Client was registered");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private async Task Login(ServiceProvider serviceProvider)
		{
			if (_client is not null)
			{
				Console.WriteLine("You are already logged in");
				return;
			}
			Console.WriteLine("Login:");
			var clientDto = new ClientLoginDto();
			Console.Write("Enter email: ");
			clientDto.Email = Console.ReadLine() ?? throw new NullReferenceException();
			Console.Write("Enter password: ");
			clientDto.Password = Console.ReadLine() ?? throw new NullReferenceException();
			using (var scope = serviceProvider.CreateScope())
			{
				var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

				try
				{
					_client = await authService.Login(clientDto);

					Console.WriteLine("You logged in");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
		private void LogOut()
		{
			Console.WriteLine(_client is not null ? "You logout" : "You cannot logout, firstly login");
			_client = null;
		}
		private async Task CheckAvailability(ServiceProvider serviceProvider)
		{
			Console.WriteLine("Checking Room Availability:");

			using (var scope = serviceProvider.CreateScope())
			{
				var availabilityService = scope.ServiceProvider.GetRequiredService<IAvailabilityService>();

				Console.Write("Enter start date (yyyy-MM-dd): ");
				if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
				{
					Console.WriteLine("Invalid date format");
					return;
				}

				Console.Write("Enter end date (yyyy-MM-dd): ");
				if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
				{
					Console.WriteLine("Invalid date format");
					return;
				}

				try
				{
					var availableRooms = await availabilityService.GetAvailableRooms(startDate, endDate);

					Console.WriteLine("Available Rooms:");
					foreach (var room in availableRooms)
					{
						Console.WriteLine($"Room Number: {room.RoomNumber}, Category: {room.Category.Name}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private async Task BookRoom(ServiceProvider serviceProvider)
		{
			Console.WriteLine("Booking a Room:");

			using (var scope = serviceProvider.CreateScope())
			{
				var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

				Console.Write("Enter client Id: ");
				if (!Guid.TryParse(Console.ReadLine(), out var clientId))
				{
					Console.WriteLine("Invalid client Id format");
					return;
				}

				Console.Write("Enter room Id: ");
				if (!Guid.TryParse(Console.ReadLine(), out var roomId))
				{
					Console.WriteLine("Invalid room Id format");
					return;
				}

				Console.Write("Enter start date (yyyy-MM-dd): ");
				if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
				{
					Console.WriteLine("Invalid date format");
					return;
				}

				Console.Write("Enter end date (yyyy-MM-dd): ");
				if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
				{
					Console.WriteLine("Invalid date format");
					return;
				}

				try
				{
					var booked = await bookingService.BookRoom(clientId, roomId, startDate, endDate);

					if (booked)
					{
						Console.WriteLine("Room booked successfully");
					}
					else
					{
						Console.WriteLine("Room not available for the specified dates");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
		private async Task CreateRoom(ServiceProvider serviceProvider)
		{
			Console.WriteLine("Creating a Room:");

			using (var scope = serviceProvider.CreateScope())
			{
				var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
				var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

				Console.Write("Enter room number: ");
				string roomNumber = Console.ReadLine() ?? throw new NullReferenceException();

				Console.Write("Enter category Id: ");
				if (!Guid.TryParse(Console.ReadLine(), out var categoryId))
				{
					Console.WriteLine("Invalid category Id format");
					return;
				}

				Console.Write("Enter price per night: ");
				if (!decimal.TryParse(Console.ReadLine(), out var pricePerNight))
				{
					Console.WriteLine("Invalid price format");
					return;
				}

				try
				{
					var categoryExists = await categoryService.GetAllCategories();
					if (!categoryExists.Any(c => c.Id == categoryId))
					{
						Console.WriteLine("Category does not exist. Please create the category first.");
						return;
					}

					var roomId = await roomService.CreateRoom(roomNumber, categoryId, pricePerNight);

					Console.WriteLine($"Room created successfully. Room Id: {roomId}");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private async Task CreateCategory(ServiceProvider serviceProvider)
		{
			Console.WriteLine("Creating a Category:");

			using (var scope = serviceProvider.CreateScope())
			{
				var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

				Console.Write("Enter category name: ");
				string categoryName = Console.ReadLine() ?? throw new NullReferenceException();

				Console.Write("Enter category price coefficient: ");
				float categoryPriceCoefficient;

				if (!float.TryParse(Console.ReadLine(), out categoryPriceCoefficient))
				{
					Console.WriteLine("Invalid category price coefficient format");
					return;
				}

				try
				{
					var categoryId = await categoryService.CreateCategory(categoryName, categoryPriceCoefficient);

					Console.WriteLine($"Category created successfully. Category Id: {categoryId}");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}
