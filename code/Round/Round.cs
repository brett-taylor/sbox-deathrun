using Sandbox;
using SBoxDeathrun.Pawn;

namespace SBoxDeathrun.Round
{
	public abstract class Round
	{
		public abstract RoundTimeLimit TimeLimit { get; }
		public bool ShouldEnd { get; set; }
		public abstract RoundType RoundType { get; }
		public abstract RoundType NextRound { get; }
		public virtual bool PawnsFrozen => false;

		protected Round()
		{
			ShouldEnd = false;
		}

		public abstract void RoundStart();
		public abstract void RoundEnd();
		public abstract void RoundUpdate();

		public virtual void ClientJoined( Client client )
		{
			CreateFreeCameraPawn( client );
		}

		public void ClientKilled( Client client )
		{
			CreateFreeCameraPawn( client );
		}

		private static void CleanUpExistingPawn( Client client )
		{
			if ( client.Pawn.IsValid() )
				client.Pawn.Delete();
		}

		protected static FreeCameraPawn CreateFreeCameraPawn( Client client )
		{
			CleanUpExistingPawn( client );
			var fcp = new FreeCameraPawn();
			client.Pawn = fcp;
			fcp.Respawn();
			return fcp;
		}

		protected static PlayerPawn CreatePlayerPawn( Client client )
		{
			CleanUpExistingPawn( client );
			var pp = new PlayerPawn();
			client.Pawn = pp;
			pp.Respawn();
			return pp;
		}

		public override string ToString() => $"Round[RoundType={RoundType}, NextRound={NextRound} ShouldEnd={ShouldEnd}, TL={TimeLimit} ]";
	}
}
