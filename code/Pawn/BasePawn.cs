using Sandbox;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Pawn
{
	public abstract class BasePawn : Player
	{
		public override void Respawn()
		{
			base.Respawn();
			Event.Run( DeathrunEvents.PAWN_SPAWNED, this );
		}
	}
}
