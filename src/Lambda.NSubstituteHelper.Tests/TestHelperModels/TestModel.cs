namespace Lambda.NSubstituteHelper.Tests.TestHelperModels
{
	public class TestModel
	{
		public ITestService TestService2 { get; }
		public ITestService TestService { get; }
		public ISecondService SecondService { get; }

		public TestModel(ITestService testService, ISecondService secondService)
		{
			TestService = testService;
			SecondService = secondService;
		}

		public TestModel(ITestService testService)
		{
		}

		public TestModel(ITestService testService, ITestService testService2)
		{
			TestService = testService;
			TestService2 = testService2;
		}
	}
}
