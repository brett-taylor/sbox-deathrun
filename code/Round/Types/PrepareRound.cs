using System;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Round.Types
{
	public class PrepareRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( RoundConfig.PREPARE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.PREPARE;
		public override RoundType NextRound => RoundType.ACTIVE;
		public override bool PlayersFrozen => true;

		public override void ClientJoined( Client client )
		{
			var dp = CreateAlivePlayer( client );
			dp.Team = TeamType.RUNNER;
		}

		public override void RoundStart()
		{
			var chosenDeath = Client.All.OrderBy( _ => Guid.NewGuid() ).First();
			foreach ( var client in Client.All )
			{
				var ap = CreateAlivePlayer( client );
				ap.Team = client == chosenDeath ? TeamType.DEATH : TeamType.RUNNER;
			}
		}

		public override void RoundEnd() { }

		public override void RoundUpdate() { }
	}
}
