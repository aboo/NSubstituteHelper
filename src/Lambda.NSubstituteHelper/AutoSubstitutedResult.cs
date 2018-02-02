using System.Collections.Generic;

namespace Lambda.NSubstituteHelper
{
	/// <summary>
	/// Defines an instance of Auto Substitute Result which holds the result of substitute.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AutoSubstitutedResult<T> where T : class
	{
		/// <summary>
		/// Contains all the instances of the substitutes created.
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
		/// <returns>The instance</returns>
		public T2 Get<T2>() where T2 : class
		{
			var key = typeof(T2).ToString();
			var value = Substitutes[key];
			return value as T2;
		}
	}
}
