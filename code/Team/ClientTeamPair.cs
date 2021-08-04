using Sandbox;

namespace SBoxDeathrun.Team
{
	public struct ClientTeamPair
	{
		public int NetworkIdent { get; private set; }
		public TeamType Team { get; private set; }

		public ClientTeamPair( Client client, TeamType team )
		{
			NetworkIdent = client.NetworkIdent;
			Team = team;
		}
	}
}
