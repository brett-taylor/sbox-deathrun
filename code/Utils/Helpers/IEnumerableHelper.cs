using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace SBoxDeathrun.Utils.Helpers
{
	public static class IEnumerableHelper
	{
		public static T Random<T>( IEnumerable<T> collection )
		{
			return collection.OrderBy( _ => Rand.Double() ).First();
		}
	}
}
