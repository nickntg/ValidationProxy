using ValidationProxy.Core.Interception;

namespace ValidationProxy.Example.Services
{
	public interface ICalculatorService
	{
		public int CalculateFactorial(int number);
	}

	public class CalculatorService : ICalculatorService
	{
		[Validator(0, ValidationActions.FactorialTooLarge)]
		[Validator(1, ValidationActions.FactorialTooSmall)]
		public int CalculateFactorial(int number)
		{
			if (number == 1)
			{
				return 1;
			}

			return number * CalculateFactorial(number - 1);
		}
	}
}
