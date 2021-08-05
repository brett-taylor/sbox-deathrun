using Sandbox;
using SBoxDeathrun.Pawn;
using SBoxDeathrun.Round;

namespace SBoxDeathrun.Team
{
	public class RunnerTeam : BaseTeam
	{
		protected override void PlayerSpawned( Client client, FreeCameraPawn pawn ) { }

		protected override void PlayerSpawned( Client client, PlayerPawn pawn )
		{
			pawn.Controller.SprintSpeed = GameConfig.DEATH_SPRINT_SPEED;
			pawn.Controller.WalkSpeed = GameConfig.DEATH_WALK_SPEED;
			pawn.Controller.DefaultSpeed = GameConfig.DEATH_DEFAULT_SPEED;
			pawn.Controller.GroundFriction = GameConfig.DEATH_GROUND_FRICTION;
			pawn.Controller.AirAcceleration = GameConfig.DEATH_AIR_ACCELERATION;
		}
	}
}
