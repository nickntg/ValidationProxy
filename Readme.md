# ValidationProxy

ValidationProxy is a library that allows injection of dynamic validation rules in methods. It is designed with Web API services in mind and focuses on simplicity.

## Example usage
Having the following service:
```csharp
public interface ICalculatorService
{
	public int CalculateFactorial(int number);
}

public class CalculatorService : ICalculatorService
{
	public int CalculateFactorial(int number)
	{
		if (number == 1)
		{
			return 1;
		}

		return number * CalculateFactorial(number - 1);
	}
}
```
Assume we want to prevent CalculateFactorial from being called with numbers equal or less than zero and numbers above 10. Using ValidationProxy, this is achieved as follows:

* Annotate the CalculateFactorial method.
```csharp
public const string FactorialTooSmall = "factorial-too-small";
public const string FactorialTooLarge = "factorial-too-large";

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
```
* Add the validation interceptor in the Web API configuration.
```csharp
services.AddValidatorInterceptor();
```
* Configure ICalculatorService for interception.
```csharp
services.AddProxiedScoped<ICalculatorService, CalculatorService>();
```
* Add validation actions
```csharp
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
```
Validation actions can be removed or added dynamically during the lifetime of the application.