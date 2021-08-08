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

		public override void TakeDamage( DamageInfo info )
		{
			if ( info.Attacker is BasePawn bp && bp.IsValid() )
				BasePawnToBasePawnDamage( info );
			else
				base.TakeDamage( info );
		}

		private void BasePawnToBasePawnDamage( DamageInfo info )
		{
			var ownTeam = DeathrunGame.Current.TeamManager.GetTeamForClient( GetClientOwner() );
			var attackerTeam = DeathrunGame.Current.TeamManager.GetTeamForClient( info.Attacker.GetClientOwner() );

			if ( ownTeam != attackerTeam )
				base.TakeDamage( info );
		}
	}
}
