using Sandbox;
using SBoxDeathrun.Pawn;
using SBoxDeathrun.Round;

namespace SBoxDeathrun.Team
{
	public class DeathTeam : BaseTeam
	{
		protected override void PlayerSpawned( Client client, FreeCameraPawn pawn ) { }

		protected override void PlayerSpawned( Client client, PlayerPawn pawn )
		{
			pawn.Controller.AutoJump = true;
			pawn.Controller.SprintSpeed = GameConfig.RUNNER_SPRINT_SPEED;
			pawn.Controller.WalkSpeed = GameConfig.RUNNER_WALK_SPEED;
			pawn.Controller.DefaultSpeed = GameConfig.RUNNER_DEFAULT_SPEED;
			pawn.Controller.GroundFriction = GameConfig.RUNNER_GROUND_FRICTION;
			pawn.Controller.AirAcceleration = GameConfig.RUNNER_AIR_ACCELERATION;
		}
	}
}
