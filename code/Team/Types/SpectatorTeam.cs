using Sandbox;
using SBoxDeathrun.Pawn;

namespace SBoxDeathrun.Team
{
	public class SpectatorTeam : BaseTeam
	{
		protected override void PlayerSpawned( Client client, FreeCameraPawn pawn ) { }
		
		protected override void PlayerSpawned( Client client, PlayerPawn pawn ) { }
	}
}
