using System;
using System.Threading.Tasks;
using Dependency = Unity.Attributes;

namespace Lambda.NSubstituteHelper.Tests.TestHelperModels
{
	public class TestModel
	{
		public const string FirstDependencyName = "first";
		public const string SecondDependencyName = "second";

		public ISecondService SecondService2 { get; }
		public ITestService TestService2 { get; }
		public ITestService TestService { get; }
		public ISecondService SecondService { get; }

		public TestModel(ITestService testService, ISecondService secondService)
		{
			TestService = testService;
			SecondService = secondService;
		}

		// ReSharper disable once UnusedParameter.Local
		public TestModel(ITestService testService)
		{
		}

		public TestModel(ITestService testService, ITestService testService2)
		{
			TestService = testService;
			TestService2 = testService2;
		}

		public TestModel([Dependency.Dependency(FirstDependencyName)] ISecondService secondService, [Dependency.Dependency(SecondDependencyName)]ISecondService secondService2)
		{
			SecondService = secondService;
			SecondService2 = secondService2;
		}

		public TestModel([Dependency] ISecondService secondService)
		{
			SecondService = secondService;
		}

		public virtual string TestMethod()
		{
			// This method is required to throw exception
			throw new NotImplementedException();
		}

		public virtual async Task<object> TestMethodAsync()
		{
			// This method is required to return exception
			return await Task.FromException<NotImplementedException>(new NotImplementedException());
		}
	}
}
