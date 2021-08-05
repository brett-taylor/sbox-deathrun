using Sandbox;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Round.Types
{
	public class WaitingForPlayersRound : BaseRound
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.NoLimit();
		public override RoundType RoundType => RoundType.WAITING_FOR_PLAYERS;
		public override RoundType NextRound => RoundType.PREPARE;

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
			if ( Client.All.Count >= GameConfig.MINIMUM_PLAYERS )
				ShouldEnd = true;
		}
	}
}
