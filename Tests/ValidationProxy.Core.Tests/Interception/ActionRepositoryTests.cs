using System;
using ValidationProxy.Core.Interception;
using Xunit;

namespace ValidationProxy.Core.Tests.Interception
{
	public class ActionRepositoryTests
	{
		[Fact]
		public void AddAction()
		{
			ActionRepository.ClearValidationActions();

			ActionRepository.AddValidationAction("test", _ => { });

			Assert.NotNull(ActionRepository.GetAction("test"));
		}

		[Fact]
		public void AddDuplicateAction()
		{
			ActionRepository.ClearValidationActions();

			ActionRepository.AddValidationAction("test", _ => { });
			Assert.Throws<InvalidOperationException>(() => ActionRepository.AddValidationAction("test", _ => { }));
		}

		[Fact]
		public void AddThenClear()
		{
			ActionRepository.ClearValidationActions();

			Assert.Empty(ActionRepository.GetActions());

			ActionRepository.AddValidationAction("test", _ => { });

			Assert.Single(ActionRepository.GetActions());

			ActionRepository.AddValidationAction("another test", _ => { });

			Assert.Equal(2, ActionRepository.GetActions().Count);

			ActionRepository.ClearValidationActions();

			Assert.Empty(ActionRepository.GetActions());
		}
	}
}
