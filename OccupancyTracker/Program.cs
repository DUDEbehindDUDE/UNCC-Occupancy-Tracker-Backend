using System.Security.Cryptography;
using log4net;
using OccupancyTracker.Services;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

internal class Program
{
	private static readonly ILog log = LogManager.GetLogger(typeof(Program));
	private static void Main(string[] args)
	{

		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSingleton(serviceProvider => Social704DataService.GetInstance().Result);
		builder.Services.AddSingleton(serviceProvider => SoViDataService.GetInstance().Result);
		builder.Services.AddSingleton(serviceProvider => ParkingDataService.GetInstance().Result);
		builder.Services.AddSingleton(serviceProvider => AtkinsService.GetInstance());
		builder.Services.AddSingleton(serviceProvider => OccupancySamplingService.GetInstance());

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddSwaggerGen();
		builder.Services.AddControllers();

		builder.Services.AddCors();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		// app.UseHttpsRedirection();
		app.UseCors(x => x
			.AllowAnyMethod()
			.AllowAnyHeader()
			.SetIsOriginAllowed(origin => true) // allow any origin
		);

		app.UseAuthorization();
		app.MapControllers();

		InitializeServices();
		app.Run();
	}

	static void InitializeServices()
	{
		log.Debug("Initializing services");
		_ = Social704DataService.GetInstance();
		_ = SoViDataService.GetInstance();
		_ = ParkingDataService.GetInstance();
		_ = AtkinsService.GetInstance();
		_ = OccupancySamplingService.GetInstance();
	}
}