namespace SBoxDeathrun.Round
{
	public partial class RoundTimeLimit
	{
		public float Limit { get; private set; }
		public bool HasLimit { get; private set; }

		private RoundTimeLimit( bool hasLimit, float limit )
		{
			HasLimit = hasLimit;
			Limit = limit;
		}

		public static RoundTimeLimit WithLimit( float limit ) => new(true, limit);
		public static RoundTimeLimit NoLimit() => new(false, 0f);

		public override string ToString() => $"RoundTimeLimit[HasLimit={HasLimit}, Limit={Limit}]";
	}
}
