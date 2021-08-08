namespace SBoxDeathrun.Utils.Extensions
{
	public static class StringExtensions
	{
		public static string Plural( this string text, int count ) => text.Plural( count, $"{text}s" );
		public static string Plural( this string text, int count, string plural ) => count < 2 ? text : plural;
	}
}
