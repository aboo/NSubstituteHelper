// <copyright file="AutoSubstitutedResult.cs" company="Lambda Solutions">
// Developed under Apache-2.0 license
// </copyright>

namespace Lambda.NSubstituteHelper
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Defines an instance of Auto Substitute Result which holds the result of substitute.
	/// </summary>
	/// <typeparam name="T">The type of the target</typeparam>
	public class AutoSubstitutedResult<T>
		where T : class
	{
		/// <summary>
		/// Gets or sets the instances of the substitutes created.
		/// </summary>
		public Dictionary<string, object> Substitutes { get; set; } = new Dictionary<string, object>();

		/// <summary>
		/// Gets or sets the instance of the class with injections.
		/// </summary>
		public T Target { get; set; }

		/// <summary>
		/// Returns an instance of type T2
		/// </summary>
		/// <typeparam name="T2">The interface type of the instance</typeparam>
		/// <param name="dependencyName">The name of the dependency in case multiple instances of the same interface are injected</param>
		/// <returns>The instance</returns>
		public T2 Get<T2>(string dependencyName = null)
			where T2 : class
		{
			var key = Utility.GetSimpleKey(typeof(T2), dependencyName);
			if (!Substitutes.ContainsKey(key))
			{
				key = Utility.GetCombinedKey(typeof(T2), dependencyName);
				if (!Substitutes.ContainsKey(key))
				{
					throw new Exception("I cannot find an instance for the given interface. Try name dependency.");
				}
			}

			var value = Substitutes[key];
			return value as T2;
		}
	}
}
