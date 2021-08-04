using System;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Round.Types
{
	public class PrepareRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( GameConfig.PREPARE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.PREPARE;
		public override RoundType NextRound => RoundType.ACTIVE;
		public override bool PawnsFrozen => true;

		public override void ClientJoined( Client client )
		{
			DeathrunGame.Current.TeamManager.AddClientToTeam( client, TeamType.RUNNER );
			CreatePlayerPawn( client );
		}

		public override void RoundStart()
		{
			var chosenDeath = Client.All.OrderBy( _ => Guid.NewGuid() ).First();
			Log.Info( $"Deaths chosen: {chosenDeath.Name}, had {Client.All.Count} options." );

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
