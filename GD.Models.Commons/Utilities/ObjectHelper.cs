namespace GD.Models.Commons.Utilities
{
	public static class ObjetHelper
	{
		/// <summary>
		/// Validates if a objet is null or empty
		/// </summary>
		/// <param name="obj">Object of any type</param>
		/// <returns>bool</returns>
		public static bool IsNullOrEmpty(this object obj)
		{
			return obj == null;
		}

		/// <summary>
		/// Validates if a number of type long is greater than zero
		/// </summary>
		/// <param name="obj">Parameter of type long</param>
		/// <returns>bool</returns>
		public static bool IsGreaterThanZero(this long obj)
		{
			return obj > 0;
		}

		/// <summary>
		/// Validates if a number of type int is greater than zero
		/// </summary>
		/// <param name="obj">Parameter of type int</param>
		/// <returns>bool</returns>
		public static bool IsGreaterThanZero(this int obj)
		{
			return obj > 0;
		}

		/// <summary>
		/// Validates if a number of type double is greater than zero
		/// </summary>
		/// <param name="obj">Parameter of type double</param>
		/// <returns>bool</returns>
		public static bool IsGreaterThanZero(this double obj)
		{
			return obj > 0;
		}

		/// <summary>
		/// Validates if a number of type decimal is greater than zero
		/// </summary>
		/// <param name="obj">Parameter of type decimal</param>
		/// <returns>bool</returns>
		public static bool IsGreaterThanZero(this decimal obj)
		{
			return obj > 0;
		}

		/// <summary>
		/// Validates if a number of type float is greater than zero
		/// </summary>
		/// <param name="obj">Parameter of type float</param>
		/// <returns>bool</returns>
		public static bool IsGreaterThanZero(this float obj)
		{
			return obj > 0;
		}
	}
}
