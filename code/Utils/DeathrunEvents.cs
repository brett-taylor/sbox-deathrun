namespace SBoxDeathrun.Utils
{
	public static class DeathrunEvents
	{
		public const string ROUND_ANY_STARTED = "deathrun.round.any.started"; // No Params,
		public const string ROUND_ANY_COMPLETED = "deathrun.round.any.completed"; // No Params
		public const string ROUND_WAITING_FOR_PLAYERS_STARTED = "deathrun.round.waiting_for_players.started"; // No Params,
		public const string ROUND_WAITING_FOR_PLAYERS_COMPLETED = "deathrun.round.waiting_for_players.completed"; // No Params
		public const string ROUND_PREPARE_STARTED = "deathrun.round.prepare.started"; // No Params,
		public const string ROUND_PREPARE_COMPLETED = "deathrun.round.prepare.completed"; // No Params
		public const string ROUND_ACTIVE_STARTED = "deathrun.round.active.started"; // No Params,
		public const string ROUND_ACTIVE_COMPLETED = "deathrun.round.active.completed"; // No Params,
		public const string ROUND_POST_STARTED = "deathrun.round.post.started"; // No Params,
		public const string ROUND_POST_COMPLETED = "deathrun.round.post.completed"; // No Params

		public const string PAWN_SPAWNED = "deathrun.pawn.spawned"; // Param1: BasePawn
	}
}
