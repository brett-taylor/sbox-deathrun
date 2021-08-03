using Sandbox;
using SBoxDeathrun.Player;

namespace SBoxDeathrun.Round
{
	public abstract class Round
	{
		public abstract RoundTimeLimit TimeLimit { get; }
		public bool ShouldEnd { get; set; }
		public abstract RoundType RoundType { get; }
		public abstract RoundType NextRound { get; }

		protected Round()
		{
			ShouldEnd = false;
		}

		public abstract void RoundStart();
		public abstract void RoundEnd();
		public abstract void RoundUpdate();

		public virtual void ClientJoined( Client client )
		{
			CreateSpectatorPlayer( client );
		}

		private static void CleanUpExistingPawn( Client client )
		{
			if ( client.Pawn.IsValid() )
				client.Pawn.Delete();
		}

		protected static void CreateSpectatorPlayer( Client client )
		{
			CleanUpExistingPawn( client );
			var sp = new SpectatorPlayer();
			client.Pawn = sp;
			sp.Respawn();
		}

		protected static void CreateDeathrunPlayer( Client client )
		{
			CleanUpExistingPawn( client );
			var dp = new DeathrunPlayer();
			client.Pawn = dp;
			dp.Respawn();
		}

		public virtual void ClientKilled( Client client )
		{
			CreateSpectatorPlayer( client );
		}

		public override string ToString() => $"Round[RoundType={RoundType}, NextRound={NextRound} ShouldEnd={ShouldEnd}, TL={TimeLimit} ]";
	}
}
