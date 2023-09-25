using System;
using ValidationProxy.Core.Extensions;
using ValidationProxy.Core.Models;
using Xunit;

namespace ValidationProxy.Core.Tests.Extensions
{
	public class InvocationArgumentExtensionsTests
	{
		[Fact]
		public void GetInvocationArgumentByPosition()
		{
			var info = GetInvocationInfo();

			Assert.Equal("string", info.ArgumentByPosition(0));
			Assert.Equal(1, info.ArgumentByPosition(1));
			Assert.Equal(true, info.ArgumentByPosition(2));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(3)]
		public void GetInvocationArgumentByPositionOutOfRange(int position)
		{
			var info = GetInvocationInfo();

			Assert.Throws<ArgumentOutOfRangeException>(() => info.ArgumentByPosition(position));
		}

		[Fact]
		public void GetInvocationArgumentByName()
		{
			var info = GetInvocationInfo();

			Assert.Equal("string", info.ArgumentByName("arg1"));
			Assert.Equal(1, info.ArgumentByName("arg2"));
			Assert.Equal(true, info.ArgumentByName("arg3"));
		}

		[Fact]
		public void GetInvocationArgumentByNameNotFound()
		{
			var info = GetInvocationInfo();

			Assert.Throws<ArgumentException>(() => info.ArgumentByName("invalid"));
		}

		[Fact]
		public void ArgumentAsString()
		{
			var info = GetInvocationInfo();

			Assert.Equal("string", info.ArgumentByName("arg1").AsString());
			Assert.Equal("string", info.ArgumentByPosition(0).AsString());
		}

		[Fact]
		public void ArgumentAsInt()
		{
			var info = GetInvocationInfo();

			Assert.Equal(1, info.ArgumentByName("arg2").AsInt());
			Assert.Equal(1, info.ArgumentByPosition(1).AsInt());
		}

		[Fact]
		public void ArgumentAsIntThrows()
		{
			var info = GetInvocationInfo();

			Assert.Throws<ArgumentException>(() => info.ArgumentByName("arg1").AsInt());
			Assert.Throws<ArgumentException>(() => info.ArgumentByPosition(0).AsInt());
		}

		[Fact]
		public void ArgumentAsBoolean()
		{
			var info = GetInvocationInfo();

			Assert.True(info.ArgumentByName("arg3").AsBoolean());
			Assert.True(info.ArgumentByPosition(2).AsBoolean());

			info.InvocationArguments[2] = false;

			Assert.False(info.ArgumentByName("arg3").AsBoolean());
			Assert.False(info.ArgumentByPosition(2).AsBoolean());
		}

		[Fact]
		public void ArgumentAsBooleanThrows()
		{
			var info = GetInvocationInfo();

			Assert.Throws<ArgumentException>(() => info.ArgumentByName("arg1").AsBoolean());
			Assert.Throws<ArgumentException>(() => info.ArgumentByPosition(0).AsBoolean());
		}

		private InvocationInfo GetInvocationInfo()
		{
			return new InvocationInfo
			{
				InvocationArguments = new object[] { "string", 1, true },
				MethodInfo = typeof(MyService).GetMethod("MyMethod"),
			};
		}
	}

	internal class MyService
	{
		public void MyMethod(string arg1, int arg2, bool arg3)
		{
		} 
	}
}
