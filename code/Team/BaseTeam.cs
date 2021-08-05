using Sandbox;
using SBoxDeathrun.Pawn;

namespace SBoxDeathrun.Team
{
	public abstract class BaseTeam
	{
		public void PlayerSpawned( Client client, BasePawn pawn )
		{
			switch ( pawn )
			{
				case FreeCameraPawn fp:
					PlayerSpawned( client, fp );
					break;
				case PlayerPawn pp:
					PlayerSpawned( client, pp );
					break;
			}
		}

		protected abstract void PlayerSpawned( Client client, FreeCameraPawn pawn );
		protected abstract void PlayerSpawned( Client client, PlayerPawn pawn );
	}
}
