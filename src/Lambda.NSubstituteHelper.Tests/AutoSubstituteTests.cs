using System.Collections.Generic;
using Lambda.NSubstituteHelper.Tests.TestHelperModels;
using NFluent;
using NSubstitute;
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
				AutoSubstitute.For<TestModel>(constructorIndex);
			}).ThrowsAny();
		}

		[Fact]
		public void For_DoesntThrowsException_WhenMoreThatOneCopyOfSameInterfaceIsInjectedWithNameDependency()
		{
			// arrange 
			const int constructorIndex = 3;

			// act
			Check.ThatCode(() =>
			{
				AutoSubstitute.For<TestModel>(constructorIndex);
			}).DoesNotThrow();
		}

		[Fact]
		public void For_UsesTheType_WhenAWrongDependencyAttributeIsUsed()
		{
			// arrange 
			const int constructorIndex = 4;

			// act
			Check.ThatCode(() =>
			{
				AutoSubstitute.For<TestModel>(constructorIndex);
			}).DoesNotThrow();
		}

		[Fact]
		public void For_DotNotThrow_WhenInstancesToUseIsNull()
		{
			// arrange & arrange & assert
			Check.ThatCode(() =>
			{
				AutoSubstitute.For<TestModel>();
			}).DoesNotThrow();
		}

		[Fact]
		public void For_DotNotThrow_WhenInstancesToUseIsNotNullAndInstanceIsNotProvided()
		{
			// arrange & arrange & assert
			Check.ThatCode(() =>
			{
				AutoSubstitute.For<TestModel>(instancesToUse: new Dictionary<string, object>());
			}).DoesNotThrow();
		}

		[Fact]
		public void For_ReturnsPreDefinedInstance_WhenInstancesArePassed()
		{
			// arrange 
			var expectedTestService = Substitute.For<ITestService>();
			var instancesToUse = new Dictionary<string, object>
			{
				{typeof(ITestService).ToString(), expectedTestService}
			};
			var autoSubstituteResult = AutoSubstitute.For<TestModel>(instancesToUse: instancesToUse);

			// act
			var actualTestService = autoSubstituteResult.Get<ITestService>();

			// assert
			Check.That(actualTestService).Equals(expectedTestService);
		}

		[Fact]
		public void For_ReturnsPreDefinedInstance_WhenInstancesArePassedWithDependencyName()
		{
			// arrange 
			const string firstDependencyName = "first";
			const string secondDependencyName = "second";
			const int constructorIndex = 3;
			var expectedFirstService = Substitute.For<ISecondService>();
			var expectedSecondService = Substitute.For<ISecondService>();
			var instancesToUse = new Dictionary<string, object>
			{
				{firstDependencyName, expectedFirstService},
				{secondDependencyName, expectedSecondService }
			};
			var autoSubstituteResult = AutoSubstitute.For<TestModel>(instancesToUse: instancesToUse, constructorIndex: constructorIndex);

			// act
			var actualFirstService = autoSubstituteResult.Get<ISecondService>(firstDependencyName);
			var actualSecondService = autoSubstituteResult.Get<ISecondService>(secondDependencyName);

			// assert
			Check.That(actualFirstService).Equals(expectedFirstService);
			Check.That(actualSecondService).Equals(expectedSecondService);
		}
	}
}
