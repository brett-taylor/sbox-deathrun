using Sandbox;
using SBoxDeathrun.Pawn;
using SBoxDeathrun.Team;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Round.Types
{
	public class WaitingForPlayersRound : BaseRound
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.NoLimit();
		public override RoundType RoundType => RoundType.WAITING_FOR_PLAYERS;
		public override RoundType NextRound => RoundType.PREPARE;
		public override string RoundStartEventName => DeathrunEvents.ROUND_WAITING_FOR_PLAYERS_STARTED;
		public override string RoundCompletedEventName => DeathrunEvents.ROUND_WAITING_FOR_PLAYERS_COMPLETED;

		public override void ClientJoined( Client client )
		{
			DeathrunGame.Current.TeamManager.AddClientToTeam( client, TeamType.DEATH );
			CreateFreeCameraPawn( client );

			var dcp = new DeathPathCameraPawn();
			client.Pawn = dcp;
			dcp.Respawn();
		}

		public override void RoundStart()
		{
			foreach ( var client in Client.All )
			{
				DeathrunGame.Current.TeamManager.AddClientToTeam( client, TeamType.SPECTATOR );
				CreateFreeCameraPawn( client );
			}
		}

		public override void RoundEnd() { }

		public override void RoundUpdate()
		{
			ShouldEnd = Client.All.Count >= GameConfig.MINIMUM_PLAYERS;
		}
	}
}
