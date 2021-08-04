using System;

namespace SBoxDeathrun.Utils
{
	public static class RandomEnum
	{
		private static readonly Random Random = new();

		public static T Of<T>()
		{
			var v = Enum.GetValues( typeof(T) );
			return (T)v.GetValue( Random.Next( v.Length ) );
		}
	}
}
