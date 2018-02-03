using System.Collections.Generic;
using Lambda.NSubstituteHelper.Tests.TestHelperModels;
using NFluent;
using Xunit;
using NSubstitute;

namespace Lambda.NSubstituteHelper.Tests
{
	public class AutoSubstituteResultTests
	{
		[Fact]
		public void Properties_Check()
		{
			// arrange & act
			const string key = "key";
			var value = new object();
			var target = new object();

			var substitutes = new Dictionary<string, object>
			{
				{key, value}
			};

			var model = new AutoSubstitutedResult<object>
			{
				Substitutes = substitutes,
				Target = target
			};

			// assert
			Check.That(model).HasFieldsWithSameValues(new
			{
				Substitutes = substitutes,
				Target = target
			});

			Check.That(model.Target).Equals(target);
			Check.That(model.Substitutes).Equals(substitutes);
		}

		[Fact]
		public void Substitutes_Instantiates_ByDefault()
		{
			// arrange & act
			var model = new AutoSubstitutedResult<object>();

			// assert
			Check.That(model.Substitutes).IsNotNull();
		}

		[Fact]
		public void Get_ReturnsTheInstance_WhenASubstituteInstanceExists()
		{
			// arrange
			var model = new AutoSubstitutedResult<object>();
			var expected = Substitute.For<ITestService>();
			model.Substitutes.Add(typeof(ITestService).ToString(), expected);

			// act
			var actual = model.Get<ITestService>();

			// assert
			Check.That(actual).Equals(expected);
		}

		[Fact]
		public void Get_Throws_WhenKeyDoesntExist()
		{
			// arrange
			const int constructorIndex = 3;
			var autoSubstituteResult = AutoSubstitute.For<TestModel>(constructorIndex);

			// act & assert
			Check.ThatCode(() => { autoSubstituteResult.Get<ISecondService>(); }).ThrowsAny();
		}

		[Fact]
		public void Get_WillReturnDifferentInstances_WhenCopiesOfSameInterfaceIsInjected()
		{
			// arrange
			const string firstDependencyName = "first";
			const string secondDependencyName = "second";
			const int constructorIndex = 3;
			var autoSubstituteResult = AutoSubstitute.For<TestModel>(constructorIndex);

			// act
			var firstInstance = autoSubstituteResult.Get<ISecondService>(firstDependencyName);
			var secondService = autoSubstituteResult.Get<ISecondService>(secondDependencyName);

			// assert
			Check.That(firstInstance).Not.Equals(secondService);
		}
	}
}
