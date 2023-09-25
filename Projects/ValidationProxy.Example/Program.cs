using Microsoft.OpenApi.Models;
using System.Reflection;
using ValidationProxy.Core.Extensions;
using ValidationProxy.Core.Interception;
using ValidationProxy.Example.Services;

namespace ValidationProxy.Example
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();

			ConfigureSwagger(builder.Services);
			ConfigureValidationProxy(builder.Services);

			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		private static void ConfigureValidationProxy(IServiceCollection services)
		{
			services.AddValidatorInterceptor()
				.AddProxiedScoped<ICalculatorService, CalculatorService>();

			ActionRepository.AddValidationAction(ValidationActions.FactorialTooSmall, info =>
			{
				var argument = info.ArgumentByPosition(0).AsInt();
				if (argument<=0)
				{
					throw new ArgumentException($"Cannot calculate factorial for {argument}");
				}
			});

			ActionRepository.AddValidationAction(ValidationActions.FactorialTooLarge, info =>
			{
				var argument = info.ArgumentByName("number").AsInt();
				if (argument > 10)
				{
					throw new ArgumentException($"Will not calculate factorial for {argument}");
				}
			});
		}

		private static void ConfigureSwagger(IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Validation Proxy Example",
					Description = "An ASP.NET to demonstrate ValidationProxy"
				});

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
			});
		}
	}
}