using System;
using System.Collections.Generic;

#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace Lambda.NSubstituteHelper
{
	public class AutoSubstitute
	{
		public static AutoSubstitutedResult<T> For<T>(int constructorIndex = 0) where T : class
		{
			var result = new AutoSubstitutedResult<T>();

			var constructor = typeof(T).GetConstructors()[constructorIndex];
			var parameters = constructor.GetParameters();
			var parametersObjects = new List<object>();

			foreach (var parameter in parameters)
			{
				var type = parameter.ParameterType;
				var key = type.ToString();

				var substitute = NSubstitute.Substitute.For(new[]
				{
					type
				}, null);


				parametersObjects.Add(substitute);

				if (result.Substitutes.ContainsKey(key))
				{
					throw new Exception("Multiple copies of the same interface is being used. To avoid this use DependencyAttribute injection");
				}

				result.Substitutes.Add(key, substitute);
			}

			result.Target = (T)Activator.CreateInstance(typeof(T), parametersObjects.ToArray());
			return result;
		}
	}
}
