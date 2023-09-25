using Microsoft.AspNetCore.Mvc;
using ValidationProxy.Example.Services;

namespace ValidationProxy.Example.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CalculatorController : ControllerBase
	{
		private readonly ICalculatorService _calculatorService;
		private readonly ILogger<CalculatorController> _logger;

		public CalculatorController(ICalculatorService calculatorService,
			ILogger<CalculatorController> logger)
		{
			_calculatorService = calculatorService;
			_logger = logger;
		}

		[HttpGet]
		[Route("factorial")]
		public IActionResult CalculateFactorial(int number)
		{
			_logger.LogInformation($"Calculate factorial for {number}");

			return Ok(_calculatorService.CalculateFactorial(number));
		}
	}
}
