using System;
using Sandbox;

namespace SBoxDeathrun.Utils.Helpers
{
	public static class EnumHelper
	{
		public static T Of<T>()
		{
			var v = Enum.GetValues( typeof(T) );
			return (T)v.GetValue( Rand.Int( v.Length ) );
		}
	}
}
