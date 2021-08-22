using System.Linq;
using Sandbox;
using SBoxDeathrun.Team;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Round.Types
{
	public class ActiveRound : BaseRound
	{
		[ConVar.ReplicatedAttribute( "deathrun_round_active_length" )]
		public static float ACTIVE_ROUND_LENGTH { get; set; } = 60f;
		
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( ACTIVE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.ACTIVE;
		public override RoundType NextRound => RoundType.POST;
		public override string RoundStartEventName => DeathrunEvents.ROUND_ACTIVE_STARTED;
		public override string RoundCompletedEventName => DeathrunEvents.ROUND_ACTIVE_COMPLETED;

		public override void RoundStart() { }

		public override void RoundEnd()
		{
			var numberOfAliveRunners = NumberOfAliveRunners();
			var numberOfAliveDeaths = NumberOfAliveDeaths();

			var outcome = (numberOfAliveRunners > 0, numberOfAliveDeaths > 0) switch
			{
				(false, false) => ActiveRoundOutcome.TIED, // Both teams had 0 pawns alive (Everyone left?)
				(true, true) => ActiveRoundOutcome.TIED, // Both teams had X pawns alive (Ran out of time?)
				(true, false) => ActiveRoundOutcome.RUNNERS_WIN, // Runners had someone alive while Deaths did not
				(false, true) => ActiveRoundOutcome.DEATHS_WIN, // Deaths had someone alive while Runners did not
			};

			DeathrunGame.Current.RoundManager.SetLastActiveRoundOutcome( outcome );
		}

		public override void RoundUpdate()
		{
			ShouldEnd = NumberOfAliveRunners() == 0 || NumberOfAliveDeaths() == 0;
		}

		private static int NumberOfAliveRunners()
		{
			return DeathrunGame.Current.TeamManager
				.GetAllClientsInTeam( TeamType.RUNNER )
				.Count( client => client.Pawn.LifeState == LifeState.Alive );
		}

		private static int NumberOfAliveDeaths()
		{
			return DeathrunGame.Current.TeamManager
				.GetAllClientsInTeam( TeamType.DEATH )
				.Count( client => client.Pawn.LifeState == LifeState.Alive );
		}
	}
}
