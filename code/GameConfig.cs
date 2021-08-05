namespace SBoxDeathrun.Round
{
	public static class GameConfig
	{
		public static readonly int MINIMUM_PLAYERS = 2;
		public static readonly float PREPARE_ROUND_LENGTH = 10f;
		public static readonly float ACTIVE_ROUND_LENGTH = 60f;
		public static readonly float FINISH_ROUND_LENGTH = 5f;

		public static readonly float DEATH_GROUND_FRICTION = 6f;
		public static readonly float DEATH_AIR_ACCELERATION = 500f;
		public static readonly float DEATH_SPRINT_SPEED = 320f;
		public static readonly float DEATH_WALK_SPEED = 320f;
		public static readonly float DEATH_DEFAULT_SPEED = 320f;

		public static readonly float RUNNER_GROUND_FRICTION = 6f;
		public static readonly float RUNNER_AIR_ACCELERATION = 500f;
		public static readonly float RUNNER_SPRINT_SPEED = 250f;
		public static readonly float RUNNER_WALK_SPEED = 250f;
		public static readonly float RUNNER_DEFAULT_SPEED = 250f;
	}
}
