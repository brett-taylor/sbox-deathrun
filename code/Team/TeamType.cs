using System;

namespace SBoxDeathrun.Team
{
	public enum TeamType
	{
		SPECTATOR,
		DEATH,
		RUNNER
	}

	public static class TeamTypeExtensions
	{
		public static string NiceName( this TeamType type )
		{
			return type switch
			{
				TeamType.SPECTATOR => "Spectators",
				TeamType.DEATH => "Deaths",
				TeamType.RUNNER => "Runners",
				_ => throw new ArgumentOutOfRangeException( nameof(type), type, null )
			};
		}
	}
}
