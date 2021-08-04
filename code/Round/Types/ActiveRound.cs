using System.Linq;
using Sandbox;
using SBoxDeathrun.Player;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Round.Types
{
	public class ActiveRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( RoundConfig.ACTIVE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.ACTIVE;
		public override RoundType NextRound => RoundType.FINISH;

		public override void RoundStart() { }

		public override void RoundEnd()
		{
			var numberOfAliveRunners = NumberOfAliveRunners();
			var numberOfAliveDeaths = NumberOfAliveDeaths();

			var outcome = (numberOfAliveRunners > 0, numberOfAliveDeaths > 0) switch
			{
				(false, false) => RoundSuccessOutcome.TIED, // Both teams had 0 players alive (Everyone left?)
				(true, true) => RoundSuccessOutcome.TIED, // Both teams had X players alive (Ran out of time?)
				(true, false) => RoundSuccessOutcome.RUNNERS_WIN, // Runners had someone alive while Deaths did not
				(false, true) => RoundSuccessOutcome.DEATHS_WIN, // Deaths had someone alive while Runners did not
			};
			
			Event.Run( DeathrunEvents.ActiveRoundCompleted, outcome );
		}

		public override void RoundUpdate()
		{
			if ( NumberOfAliveRunners() == 0 || NumberOfAliveDeaths() == 0 )
				ShouldEnd = true;
		}

		private static int NumberOfAliveRunners() => TeamedPlayer.FindAllRunners().Count( tp => tp.LifeState == LifeState.Alive );
		private static int NumberOfAliveDeaths() => TeamedPlayer.FindAllDeaths().Count( tp => tp.LifeState == LifeState.Alive );
	}
}
