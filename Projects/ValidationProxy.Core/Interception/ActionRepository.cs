using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using ValidationProxy.Core.Models;

namespace ValidationProxy.Core.Interception
{
	public class ActionRepository
	{
		private static readonly ConcurrentDictionary<string, Action<InvocationInfo>> Actions = new();

		public static void AddValidationAction(string actionKey, Action<InvocationInfo> action)
		{
			if (!Actions.TryAdd(actionKey, action))
			{
				throw new InvalidOperationException($"Action with key {actionKey} could not be added");
			}
		}

		public static void ClearValidationActions()
		{
			Actions.Clear();
		}

		public static ImmutableDictionary<string, Action<InvocationInfo>> GetActions()
		{
			return Actions.ToImmutableDictionary();
		}

		public static Action<InvocationInfo> GetAction(string actionKey)
		{
			if (!Actions.TryGetValue(actionKey, out var action))
			{
				throw new KeyNotFoundException($"Action with key {actionKey} does not exist");
			}

			return action;
		}
	}
}
