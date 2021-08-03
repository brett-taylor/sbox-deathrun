using Sandbox;
using SBoxDeathrun.Player;

namespace SBoxDeathrun
{
	public class DeathrunGame : Game
	{
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new DeathrunPlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}
}
