using System.Collections.Generic;

namespace SBoxDeathrun.Utils.Helpers
{
	public static class ListHelper
	{
		public static List<T> Empty<T>() => new();
		public static List<T> Of<T>(params T[] args) => new(args);
		
	}
}
