using System.Collections.Generic;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Player
{
	public partial class TeamedPlayer : Sandbox.Player
	{
		[Net] public TeamType Team { get; set; }

		public TeamedPlayer()
		{
			Team = TeamType.SPECTATOR;
		}

		public static List<TeamedPlayer> FindAllRunners() => FindByTeam( TeamType.RUNNER );
		public static List<TeamedPlayer> FindAllDeaths() => FindByTeam( TeamType.DEATH );
		public static List<TeamedPlayer> FindAllSpectators() => FindByTeam( TeamType.SPECTATOR );

		public static List<TeamedPlayer> FindByTeam( TeamType team )
		{
			return All
				.Where( entity => entity is TeamedPlayer tp && tp.Team == team )
				.Cast<TeamedPlayer>()
				.ToList();
		}
	}
}
