using Lambda.NSubstituteHelper.Tests.TestHelperModels;
using NFluent;
using Xunit;

namespace Lambda.NSubstituteHelper.Tests
{
	public class AutoSubstituteTests
	{
		[Fact]
		public void For_Returns_NotNullResult()
		{
			// arrange & act
			var autoSubstituteResult = AutoSubstitute.For<TestModel>();

			// assert
			Check.That(autoSubstituteResult).IsNotNull();
		}

		[Fact]
		public void For_ReturnsResult_WithCorrectTarget()
		{
			// arrange
			var autoSubstituteResult = AutoSubstitute.For<TestModel>();

			// act
			var model = autoSubstituteResult.Target;

			// assert
			Check.That(model).IsNotNull();
			Check.That(model).IsInstanceOf<TestModel>();
		}

		[Fact]
		public void For_ReturnsResult_WithCorrectNumberOfSubstitutes()
		{
			// arrange & act
			const int numberOfParameters = 2;
			var autoSubstituteResult = AutoSubstitute.For<TestModel>();

			// assert
			Check.That(autoSubstituteResult.Substitutes.Count).Equals(numberOfParameters);
		}

		[Fact]
		public void For_ReturnsResult_WithCorrectNumberOfSubstitutesForSecondConstructor()
		{
			// arrange
			const int numberOfParameters = 1;
			const int constructorIndex = 1;

			// act
			var autoSubstituteResult = AutoSubstitute.For<TestModel>(constructorIndex);

			// assert
			Check.That(autoSubstituteResult.Substitutes.Count).Equals(numberOfParameters);
		}

		[Fact]
		public void For_PassesSubstituteParamters_ToTarget()
		{
			// arrange 
			var autoSubstituteResult = AutoSubstitute.For<TestModel>();
			var expectedTestService = autoSubstituteResult.Get<ITestService>();
			var expectedSecondService = autoSubstituteResult.Get<ISecondService>();
			var model = autoSubstituteResult.Target;

			// act
			var actualTestService = model.TestService;
			var actualSecondService = model.SecondService;

			// assert
			Check.That(actualTestService).Equals(expectedTestService);
			Check.That(actualSecondService).Equals(expectedSecondService);
		}

		[Fact]
		public void For_ThrowsException_WhenMoreThatOneCopyOfSameInterfaceIsInjectedWithoutNameDependency()
		{
			// arrange 
			const int constructorIndex = 2;

			// act
			Check.ThatCode(() =>
			{
				var autoSubstituteResult = AutoSubstitute.For<TestModel>(constructorIndex);
			}).ThrowsAny();
		}
	}
}
