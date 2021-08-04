using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace SBoxDeathrun.Team
{
	public partial class TeamManager : Entity
	{
		[Net] public List<ClientTeamPair> Clients { get; private set; }

		public TeamManager()
		{
			Transmit = TransmitType.Always;
			Clients = new List<ClientTeamPair>();
		}

		public void AddClientToTeam( Client client, TeamType team )
		{
			Host.AssertServer();
			RemoveClientFromTeam( client );
			Clients.Add( new ClientTeamPair( client, team ) );
		}

		public void ClientDisconnected( Client client )
		{
			Host.AssertServer();
			RemoveClientFromTeam( client );
		}

		public TeamType GetTeamForClient( Client client )
		{
			return DoesClientTeamPairExist( client ) ? FindClientTeamPair( client ).Team : TeamType.SPECTATOR;
		}

		public IEnumerable<Client> AllRunners() => GetAllClientsInTeam( TeamType.RUNNER );
		public IEnumerable<Client> AllDeaths() => GetAllClientsInTeam( TeamType.DEATH );
		public IEnumerable<Client> AllSpectators() => GetAllClientsInTeam( TeamType.SPECTATOR );

		public IEnumerable<Client> GetAllClientsInTeam( TeamType team )
		{
			return GetClientAndTeam()
				.Where( tuple => tuple.Item2 == team )
				.Select( ct => ct.Item1 );
		}

		private IEnumerable<(Client, TeamType)> GetClientAndTeam()
		{
			return Client.All
				.Select( client => (client, FindClientTeamPair( client ).Team) );
		}

		private void RemoveClientFromTeam( Client client )
		{
			Clients.Remove( FindClientTeamPair( client ) );
		}

		private ClientTeamPair FindClientTeamPair( Client client )
		{
			foreach ( var clientTeamPair in Clients.Where( clientTeamPair => clientTeamPair.NetworkIdent == client.NetworkIdent ) )
				return clientTeamPair;

			return new ClientTeamPair( client, TeamType.SPECTATOR );
		}

		private bool DoesClientTeamPairExist( Client client )
		{
			return Clients.Any( ctp => ctp.NetworkIdent == client.NetworkIdent );
		}
	}
}
