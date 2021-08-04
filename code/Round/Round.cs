using Sandbox;
using SBoxDeathrun.Player;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Round
{
	public abstract class Round
	{
		public abstract RoundTimeLimit TimeLimit { get; }
		public bool ShouldEnd { get; set; }
		public abstract RoundType RoundType { get; }
		public abstract RoundType NextRound { get; }
		public virtual bool PlayersFrozen => false;

		protected Round()
		{
			ShouldEnd = false;
		}

		public abstract void RoundStart();
		public abstract void RoundEnd();
		public abstract void RoundUpdate();

		public virtual void ClientJoined( Client client )
		{
			var dp = CreateDeadPlayer( client );
			dp.Team = TeamType.SPECTATOR;
		}

		public void ClientKilled( Client client )
		{
			CreateDeadPlayer( client );
		}

		private static void CleanUpExistingPawn( Client client )
		{
			if ( client.Pawn.IsValid() )
				client.Pawn.Delete();
		}

		private static TeamType GetClientCurrentTeam( Client client )
		{
			if ( client.Pawn is TeamedPlayer tp )
				return tp.Team;

			return TeamType.SPECTATOR;
		}

		protected static DeadPlayer CreateDeadPlayer( Client client )
		{
			var currentTeamType = GetClientCurrentTeam( client );
			CleanUpExistingPawn( client );

			var dp = new DeadPlayer();
			dp.Team = currentTeamType;

			client.Pawn = dp;
			dp.Respawn();
			return dp;
		}

		protected static AlivePlayer CreateAlivePlayer( Client client )
		{
			var currentTeamType = GetClientCurrentTeam( client );
			CleanUpExistingPawn( client );

			var ap = new AlivePlayer();
			ap.Team = currentTeamType;

			client.Pawn = ap;
			ap.Respawn();
			return ap;
		}

		public override string ToString() => $"Round[RoundType={RoundType}, NextRound={NextRound} ShouldEnd={ShouldEnd}, TL={TimeLimit} ]";
	}
}
