namespace SBoxDeathrun.Utils
{
	public static class DeathrunEvents
	{
		public const string ROUND_STARTED = "deathrun.round.any.started"; // No Params,
		public const string ROUND_COMPLETED = "deathrun.round.any.completed"; // No Params
		public const string ROUND_ACTIVE_COMPLETED = "deathrun.round.active.completed"; // Param1: ActiveRoundOutcome

		public const string PAWN_SPAWNED = "deathrun.pawn.spawned"; // Param1: BasePawn
	}
}
