using System;

namespace SBoxDeathrun.Team
{
	public enum TeamType
	{
		DEATH,
		RUNNER,
		SPECTATOR
	}
	
	public static class TeamTypeExtensions
	{
		public static string NiceName( this TeamType type )
		{
			return type switch
			{
				TeamType.DEATH => "Death",
				TeamType.RUNNER => "Runner",
				TeamType.SPECTATOR => "Spectator",
				_ => throw new ArgumentOutOfRangeException( nameof(type), type, null )
			};
		}
	}
}
