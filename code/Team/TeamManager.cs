using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Pawn;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Team
{
	public partial class TeamManager : Entity
	{
		[Net] public List<ClientTeamPair> Clients { get; private set; }
		private readonly BaseTeam RunnerTeam = null;
		private readonly BaseTeam DeathTeam = null;
		private readonly BaseTeam SpectatorTeam = null;

		public TeamManager()
		{
			Transmit = TransmitType.Always;
			Clients = new List<ClientTeamPair>();

			if ( Host.IsServer )
			{
				RunnerTeam = new RunnerTeam();
				DeathTeam = new DeathTeam();
				SpectatorTeam = new SpectatorTeam();
			}
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

		private BaseTeam GetTeam( TeamType team )
		{
			return (team) switch
			{
				TeamType.DEATH => DeathTeam,
				TeamType.RUNNER => RunnerTeam,
				TeamType.SPECTATOR => SpectatorTeam,
				_ => throw new ArgumentOutOfRangeException( nameof(team), team, null )
			};
		}

		[Event( DeathrunEvents.PAWN_SPAWNED )]
		private void OnPawnSpawned( BasePawn basePawn )
		{
			var client = basePawn.GetClientOwner();
			if ( client.IsValid() == false )
				return;

			var team = GetTeam( DeathrunGame.Current.TeamManager.GetTeamForClient( client ) );
			team.PlayerSpawned( client, basePawn );
		}
	}
}
