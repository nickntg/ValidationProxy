using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace ValidationProxy.Core.Models
{
	public class InvocationInfo
	{
		public object[] InvocationArguments { get; set; }
		public Type[] GenericArguments { get; set; }
		public Type TargetType { get; set; }
		public MethodInfo MethodInfo { get; set; }
		public IInvocation Invocation { get; set; }
		public IServiceProvider ServiceProvider { get; set; }
	}
}
