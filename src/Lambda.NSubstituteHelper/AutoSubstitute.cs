using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace Lambda.NSubstituteHelper
{
	public class AutoSubstitute
	{
		private const string DependencyAttributeName = "DependencyAttribute";
		private const string DependencyPropertyName = "Name";

		/// <summary>
		/// Instantiates T and injects all the interfaces with substitute version.
		/// </summary>
		/// <typeparam name="T">The type of the class to instantiate</typeparam>
		/// <param name="constructorIndex">The index of the constructor to use for instantiation</param>
		/// <returns>The substitute result including the instance as well as the substitute injections</returns>
		public static AutoSubstitutedResult<T> For<T>(int constructorIndex = 0) where T : class
		{
			var result = new AutoSubstitutedResult<T>();

			var constructor = typeof(T).GetConstructors()[constructorIndex];
			var parameters = constructor.GetParameters();
			var parametersObjects = new List<object>();

			foreach (var parameter in parameters)
			{
				var type = parameter.ParameterType;
				var dependencyName = GetDependencyName(parameter);
				
				var key = dependencyName ?? type.ToString();

				var substitute = NSubstitute.Substitute.For(new[]
				{
					type
				}, null);


				parametersObjects.Add(substitute);

				if (result.Substitutes.ContainsKey(key))
				{
					throw new Exception("Multiple copies of the same interface is being used. To avoid this use DependencyAttribute injection. Or dependency name is already used.");
				}

				result.Substitutes.Add(key, substitute);
			}

			result.Target = (T)Activator.CreateInstance(typeof(T), parametersObjects.ToArray());
			return result;
		}

		private static string GetDependencyName(ParameterInfo parameter)
		{
			var attributes = parameter.GetCustomAttributes(true);
			var dependencyAttribute = (from attribute in attributes let name = attribute.GetType().Name where name.Equals(DependencyAttributeName) select attribute).FirstOrDefault();

			if (dependencyAttribute == null) return null; // there is no dependency attribute

			var nameProperty = dependencyAttribute.GetType().GetProperty(DependencyPropertyName);
			var dependencyName = nameProperty?.GetValue(dependencyAttribute) as string;

			return dependencyName;
		}
	}
}
