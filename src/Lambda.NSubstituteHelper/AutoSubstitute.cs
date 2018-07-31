// <copyright file="AutoSubstitute.cs" company="Lambda Solutions">
// Developed under Apache-2.0 license
// </copyright>

namespace Lambda.NSubstituteHelper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using NSubstitute;

	/// <summary>
	/// Defines Auto Substitute
	/// </summary>
	public class AutoSubstitute
	{
		private const string DependencyAttributeName = "DependencyAttribute";
		private const string DependencyPropertyName = "Name";

		/// <summary>
		/// Instantiates T and injects all the interfaces with substitute version.
		/// </summary>
		/// <typeparam name="T">The type of the interface to instantiate</typeparam>
		/// <param name="constructorIndex">The index of the constructor to use for instantiation</param>
		/// <param name="instancesToUse">The list of instances the needs to be used for injection rather than creating new instances</param>
		/// <returns>The substitute result including the instance as well as the substitute injections</returns>
		public static AutoSubstitutedResult<T> For<T>(int constructorIndex = 0, Dictionary<string, object> instancesToUse = null)
			where T : class
		{
			return CreateSubstitute<T>(constructorIndex, instancesToUse);
		}

		/// <summary>
		/// Instantiates parts of T and injects all the interfaces with substitute version.
		/// </summary>
		/// <typeparam name="T">The type of the class to instantiate</typeparam>
		/// <param name="constructorIndex">The index of the constructor to use for instantiation</param>
		/// <param name="instancesToUse">The list of instances the needs to be used for injection rather than creating new instances</param>
		/// <returns>The substitute result including the instance as well as the substitute injections</returns>
		public static AutoSubstitutedResult<T> ForPartsOf<T>(int constructorIndex = 0, Dictionary<string, object> instancesToUse = null)
			where T : class
		{
			return CreateSubstitute<T>(constructorIndex, instancesToUse, true);
		}

		private static string GetDependencyName(ParameterInfo parameter)
		{
			var attributes = parameter.GetCustomAttributes(true);
			var dependencyAttribute = (from attribute in attributes let name = attribute.GetType().Name where name.Equals(DependencyAttributeName) select attribute).FirstOrDefault();

			if (dependencyAttribute == null)
			{
				return null; // there is no dependency attribute
			}

			var nameProperty = dependencyAttribute.GetType().GetProperty(DependencyPropertyName);
			var dependencyName = nameProperty?.GetValue(dependencyAttribute) as string;

			return dependencyName;
		}

		private static AutoSubstitutedResult<T> CreateSubstitute<T>(int constructorIndex = 0, Dictionary<string, object> instancesToUse = null, bool usePartsOf = false)
			where T : class
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

				object substitute;
				if (instancesToUse == null || !instancesToUse.ContainsKey(key))
				{
					substitute = Substitute.For(
						new[]
						{
							type
						}, null);
					key = $"{dependencyName}_{type}";
				}
				else
				{
					substitute = instancesToUse[key];
				}

				parametersObjects.Add(substitute);

				if (result.Substitutes.ContainsKey(key))
				{
					throw new Exception("Multiple copies of the same interface is being used. To avoid this use DependencyAttribute injection. Or dependency name is already used.");
				}

				result.Substitutes.Add(key, substitute);
			}

			if (usePartsOf)
			{
				result.Target = Substitute.ForPartsOf<T>(parametersObjects.ToArray());
			}
			else
			{
				result.Target = (T)Activator.CreateInstance(typeof(T), parametersObjects.ToArray());
			}

			return result;
		}
	}
}
