using System;
using SBoxDeathrun.Round.Types;

namespace SBoxDeathrun.Round
{
	public enum RoundType
	{
		WAITING_FOR_PLAYERS,
		ACTIVE,
		PREPARE,
		POST,
	}

	public static class RoundTypeExtensions
	{
		public static BaseRound ToRound( this RoundType type )
		{
			return type switch
			{
				RoundType.WAITING_FOR_PLAYERS => new WaitingForPlayersRound(),
				RoundType.ACTIVE => new ActiveRound(),
				RoundType.PREPARE => new PrepareRound(),
				RoundType.POST => new PostRound(),
				_ => throw new ArgumentOutOfRangeException( nameof(type), type, null )
			};
		}
	}
}
