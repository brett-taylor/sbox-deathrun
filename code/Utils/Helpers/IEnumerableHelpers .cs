using System;
using System.Collections.Generic;
using System.Linq;

namespace SBoxDeathrun.Utils.Helpers
{
	public static class IEnumerableHelpers
	{
		public static T Random<T>( IEnumerable<T> collection )
		{
			var random = new Random();
			return collection.OrderBy( _ => random.NextDouble() ).First();
		}
	}
}
