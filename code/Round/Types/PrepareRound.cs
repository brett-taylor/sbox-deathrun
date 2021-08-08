using Sandbox;
using SBoxDeathrun.Team;
using SBoxDeathrun.Utils;
using SBoxDeathrun.Utils.Helpers;

namespace SBoxDeathrun.Round.Types
{
	public class PrepareRound : BaseRound
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( GameConfig.PREPARE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.PREPARE;
		public override RoundType NextRound => RoundType.ACTIVE;
		public override bool PawnsFrozen => true;
		public override string RoundStartEventName => DeathrunEvents.ROUND_PREPARE_STARTED;
		public override string RoundCompletedEventName => DeathrunEvents.ROUND_PREPARE_COMPLETED;

		public override void ClientJoined( Client client )
		{
			DeathrunGame.Current.TeamManager.AddClientToTeam( client, TeamType.RUNNER );
			CreatePlayerPawn( client );
		}

		public override void RoundStart()
		{
			var chosenDeath = IEnumerableHelpers.Random( Client.All );
			foreach ( var client in Client.All )
			{
				DeathrunGame.Current.TeamManager.AddClientToTeam( client, client == chosenDeath ? TeamType.DEATH : TeamType.RUNNER );
				CreatePlayerPawn( client );
			}
		}

		public override void RoundEnd() { }

		public override void RoundUpdate() { }
	}
}
