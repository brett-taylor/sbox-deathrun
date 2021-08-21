using Sandbox;
using SBoxDeathrun.Pawn;
using SBoxDeathrun.Round;

namespace SBoxDeathrun.Team
{
	public class RunnerTeam : BaseTeam
	{
		[ServerVar( "dr_loadout_runner_sprint_speed" )]
		public static float SPRINT_SPEED { get; set; } = 250f;
		
		[ServerVar( "dr_loadout_runner_walk_speed" )]
		public static float WALK_SPEED { get; set; } = 250f;
		
		[ServerVar( "dr_loadout_runner_default_speed" )]
		public static float DEFAULT_SPEED { get; set; } = 250f;
		
		[ServerVar( "dr_loadout_runner_ground_friction" )]
		public static float GROUND_FRICTION { get; set; } = 8f;
		
		[ServerVar( "dr_loadout_runner_air_acceleration" )]
		public static float AIR_ACCELERATION { get; set; } = 500f;
		
		protected override void PlayerSpawned( Client client, FreeCameraPawn pawn ) { }

		protected override void PlayerSpawned( Client client, PlayerPawn pawn )
		{
			pawn.Controller.AutoJump = true;
			pawn.Controller.SprintSpeed = SPRINT_SPEED;
			pawn.Controller.WalkSpeed = WALK_SPEED;
			pawn.Controller.DefaultSpeed = DEFAULT_SPEED;
			pawn.Controller.GroundFriction = GROUND_FRICTION;
			pawn.Controller.AirAcceleration = AIR_ACCELERATION;
		}
	}
}
