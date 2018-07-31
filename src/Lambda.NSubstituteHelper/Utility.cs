// <copyright file="Utility.cs" company="Lambda Solutions">
// Developed under Apache-2.0 license
// </copyright>

namespace Lambda.NSubstituteHelper
{
	using System;

	/// <summary>
	/// The utility class for some helper methods.
	/// </summary>
	public static class Utility
	{
		/// <summary>
		/// Returns the simple form of the key.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="dependencyName">The dependency name.</param>
		/// <returns>The first form key.</returns>
		public static string GetSimpleKey(Type type, string dependencyName = null)
		{
			return dependencyName ?? type.ToString();
		}

		/// <summary>
		/// Returns the combined form of the key.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="dependencyName">The dependency name.</param>
		/// <returns>The first form key.</returns>
		public static string GetCombinedKey(Type type, string dependencyName = null)
		{
			return $"{dependencyName}_{type}";
		}
	}
}
