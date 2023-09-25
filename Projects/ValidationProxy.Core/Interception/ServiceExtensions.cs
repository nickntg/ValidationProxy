using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace ValidationProxy.Core.Interception
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddValidatorInterceptor(this IServiceCollection services)
		{
			services.AddSingleton(new ProxyGenerator());
			services.AddScoped<IInterceptor, ValidationInterceptor>();
			return services;
		}

		public static IServiceCollection AddProxiedScoped<TInterface, TImplementation>
			(this IServiceCollection services)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			services.AddScoped<TImplementation>();
			services.AddScoped(typeof(TInterface), serviceProvider =>
			{
				var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
				var actual = serviceProvider.GetRequiredService<TImplementation>();
				var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
				return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
			});

			return services;
		}
	}
}