﻿using Sandbox;
using SBoxDeathrun.Entities;
using SBoxDeathrun.Round;
using SBoxDeathrun.Team;
using SBoxDeathrun.Ui;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun
{
	public partial class DeathrunGame : Game
	{
		public new static DeathrunGame Current => Game.Current as DeathrunGame;
		[Net] public RoundManager RoundManager { get; private set; }
		[Net] public TeamManager TeamManager { get; private set; }

		public DeathrunGame()
		{
			if ( Host.IsServer == false )
				return;

			RoundManager = new RoundManager();
			TeamManager = new TeamManager();
			var _ = new DeathrunHudEntity();
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
			RoundManager.Round.ClientJoined( client );
		}

		public override void ClientDisconnect( Client client, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( client, reason );
			TeamManager.ClientDisconnected( client );
		}

		[Event.Tick.ServerAttribute]
		public void Update()
		{
			if ( RoundManager is not null )
				RoundManager.Update();
		}

		public override void DoPlayerSuicide( Client cl ) { }

		public override void DoPlayerNoclip( Client player ) { }

		public override void DoPlayerDevCam( Client player ) { }

		public override void OnKilled( Client client, Entity pawn )
		{
			base.OnKilled( client, pawn );
			RoundManager.Round.ClientKilled( client );
		}

		public override void MoveToSpawnpoint( Entity pawn )
		{
			pawn.Transform = DeathrunSpawnPoint.Random().Transform;
		}

		[Event( DeathrunEvents.ROUND_ACTIVE_COMPLETED )]
		private void OnActiveRoundCompleted( ActiveRoundOutcome outcome )
		{
			Log.Warning( $"{Host.Name} DeathrunGame::OnActiveRoundCompleted called {outcome}" );
		}
	}
}
