using System;
using SBoxDeathrun.Round.Types;

namespace SBoxDeathrun.Round
{
	public enum RoundType
	{
		WAITING_FOR_PLAYERS,
		ACTIVE,
		PREPARE,
		FINISH,
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
				RoundType.FINISH => new FinishRound(),
				_ => throw new ArgumentOutOfRangeException( nameof(type), type, null )
			};
		}
	}
}
