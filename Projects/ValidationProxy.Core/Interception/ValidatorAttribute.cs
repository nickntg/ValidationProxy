using System;

namespace ValidationProxy.Core.Interception
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ValidatorAttribute : Attribute
	{
		public int Order { get; set; }
		public string ActionKey { get; set; }

		public ValidatorAttribute(int order, string actionKey)
		{
			if (string.IsNullOrEmpty(actionKey))
			{
				throw new ArgumentNullException(nameof(actionKey));
			}

			Order = order;
			ActionKey = actionKey;
		}
	}
}
