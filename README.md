# NSubstituteHelper

[![Build status](https://ci.appveyor.com/api/projects/status/cii48jqxij3saquu?svg=true)](https://ci.appveyor.com/project/aboo22424/nsubstitutehelper)


[![codecov](https://codecov.io/gh/aboo/NSubstituteHelper/branch/master/graph/badge.svg)](https://codecov.io/gh/aboo/NSubstituteHelper)

[![NuGet](https://img.shields.io/nuget/v/Lambda.NSubstituteHelper.svg)](https://www.nuget.org/packages/Lambda.NSubstituteHelper)

[![license](https://img.shields.io/github/license/aboo/NSubstituteHelper.svg)](https://github.com/aboo/NSubstituteHelper/blob/master/LICENSE)

[![GitHub last commit](https://img.shields.io/github/last-commit/aboo/NSubstituteHelper.svg)](https://github.com/aboo/NSubstituteHelper)

## What is this?
This package contains a helper for [NSubstitute](https://github.com/nsubstitute/NSubstitute). It includes helper methods to facilitate using NSubstitute and save time writing unit tests.

## Usage

### Auto Substitute
In normal cases when you inject substituted interfaces into a class constructor it usually involves a lot of maintenance. 

Issues:

- number of injected dependencies will change over time
- there could be a lot of dependencies to inject and you might not need to work with all of them in a single unit test
- the order of the dependencies will change over time and you need to go back and maintain your unit tests

Usually in order to avoid duplicate code developers put the initializer in one place and take care of all the dependencies and then share the reference. The issue there is that it will increase the unit test complexity.

**AutoSubstitute** method is here to find out all the dependencies of a class and inject them automatically.

Features:
- will inject all the dependencies automatically
- can use pre defined instances
- supports [DependencyAttribute](https://msdn.microsoft.com/en-us/library/microsoft.practices.unity.dependencyattribute.aspx) for named injection 
- choose constructor index

Limitations:
- the target class should only have interfaces injected into it
- other than DependencyAttribute it doesn't support other form of multiple injection of one interface

#### Sample 1
Simple usage 

class

    public class TestModel
    {
        public TestModel(ITestService testService)
        {
            ...
        }
    }

unit test

    var mockedModel = AutoSubstitute.For<TestModel>();
    var model = mockedModel.Target;
    var mockedTestService = mockedModel.Get<ITestService>();

#### Sample 2
Pre defined instances

class

    public class TestModel
    {
        public TestModel(IRepositoryService repositoryService, ILoggingService loggingService)
        {
            loggerService.Log("Test Model Created!");
            ...
        }
    }

unit test

    var mockedLoggerService = Substitute.For<ILoggerService>();
    mockedLoggerService.When(service => service.Log(Arg.Any<string>())).Do(info => {
        // do something
    });

    var instancesToUse = new Dictionary<string, object>
        {
            {typeof(ILoggerService).ToString(), mockedLoggerService}
        };

    var mockedModel = AutoSubstitute.For<TestModel>(instancesToUse: instancesToUse)

#### Sample 3

Use other constructors

class

    public class TestModel
    {
        public TestModel()
        {
            ...
        }

        public TestModel(ITestService testService
        {
            ...
        }
    }

unit test

    var mockedModel = AutoSubstitute.For<TestModel>(1);
    var testService = mockedModel.Get<ITestService>();

#### Sample 4
Dependency name.

class

    public class TestModel
    {
        public TestModel([Dependency("users")]IRepositoryService userRepositoryService, [Dependency("products")]IRepositoryService productRepositoryService)
        {
            ....
        }
    }

unit test

    var mockedModel = AutoSubstitute.For<TestModel>();
    var userRepository = mockedModel.Get<IRepositoryService>("users");
    var productRepository = mockedModel.Get<IRepositoryService>("products");

## How to contribute?
- Fork the repository
- Write a unit test
- Implement the change
- Make sure code coverage is 100%
- Add yourself to the Authors section
- Create a PR with enough details

## Authors
- [Aboo Azarnoush](https://twitter.com/azarnoush)
- [David Irwin](https://www.linkedin.com/in/davidmauriceirwin)