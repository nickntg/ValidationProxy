using System;
using System.Linq;
using ValidationProxy.Core.Models;

namespace ValidationProxy.Core.Extensions
{
    public static class InvocationArgumentExtensions
    {
        public static object ArgumentByPosition(this InvocationInfo invocationInfo, int argumentIndex)
        {
            if (argumentIndex < 0 || argumentIndex > invocationInfo.InvocationArguments.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(argumentIndex));
            }

            return invocationInfo.InvocationArguments[argumentIndex];
        }

        public static object ArgumentByName(this InvocationInfo invocationInfo, string argumentName)
        {
            var methodParameter = invocationInfo.MethodInfo.GetParameters().FirstOrDefault(x => x.Name == argumentName);
            var pos = methodParameter?.Position;

            if (!pos.HasValue)
            {
                throw new ArgumentException(argumentName);
            }

            return invocationInfo.InvocationArguments[pos.Value];
        }

        public static int AsInt(this object o)
        {
            if (!int.TryParse(o.ToString(), out var result))
            {
                throw new ArgumentException(o.ToString());
            }

            return result;
        }

        public static string AsString(this object o)
        {
            return o.ToString();
        }

        public static bool AsBoolean(this object o)
        {
	        if (!bool.TryParse(o.ToString(), out var result))
	        {
		        throw new ArgumentException(o.ToString());
	        }

	        return result;
        }
    }
}
