using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using ValidationProxy.Core.Models;

namespace ValidationProxy.Core.Interception
{
	public class ValidationInterceptor : IInterceptor
	{
		private readonly IServiceProvider _provider;

		public ValidationInterceptor(IServiceProvider provider)
		{
			_provider = provider;
		}

		public void Intercept(IInvocation invocation)
		{
			if (invocation.MethodInvocationTarget.GetCustomAttributes(typeof(ValidatorAttribute), false) is ValidatorAttribute[] validators && validators.Any())
			{
				var sorted = validators.OrderBy(x => x.Order);

				var info = new InvocationInfo
				{
					GenericArguments = invocation.GenericArguments,
					InvocationArguments = invocation.Arguments,
					MethodInfo = invocation.Method,
					TargetType = invocation.TargetType,
					Invocation = invocation,
					ServiceProvider = _provider
				};

				foreach (var validator in sorted)
				{
					Action<InvocationInfo> action = null;

					try
					{
						action = ActionRepository.GetAction(validator.ActionKey);
					}
					catch (KeyNotFoundException)
					{
						// Ignore and continue.
					}

					action?.Invoke(info);
				}

			}

			invocation.Proceed();
		}
	}
}
