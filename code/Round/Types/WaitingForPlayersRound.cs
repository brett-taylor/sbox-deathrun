using Sandbox;

namespace SBoxDeathrun.Round.Types
{
	public class WaitingForPlayersRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.NoLimit();
		public override RoundType RoundType => RoundType.WAITING_FOR_PLAYERS;
		public override RoundType NextRound => RoundType.PREPARE;

		public override void ClientJoined( Client client )
		{
			CreateSpectatorPlayer( client );
		}

		public override void RoundStart()
		{
			foreach ( var client in Client.All )
				CreateSpectatorPlayer( client );
		}

		public override void RoundEnd() { }

		public override void RoundUpdate()
		{
			if ( Client.All.Count >= RoundConfig.MINIMUM_PLAYERS )
				ShouldEnd = true;
		}
	}
}
