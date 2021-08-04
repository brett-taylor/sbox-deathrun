using Sandbox;

namespace SBoxDeathrun.Round.Types
{
	public class WaitingForPlayersRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.NoLimit();
		public override RoundType RoundType => RoundType.WAITING_FOR_PLAYERS;
		public override RoundType NextRound => RoundType.PREPARE;

		public override void RoundStart()
		{
			foreach ( var client in Client.All )
				CreateFreeCameraPawn( client );
		}

		public override void RoundEnd() { }

		public override void RoundUpdate()
		{
			if ( Client.All.Count >= GameConfig.MINIMUM_PLAYERS )
				ShouldEnd = true;
		}
	}
}
