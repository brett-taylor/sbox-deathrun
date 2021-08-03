using Sandbox;
using SBoxDeathrun.Round;

namespace SBoxDeathrun
{
	public partial class DeathrunGame : Game
	{
		public new static DeathrunGame Current => Game.Current as DeathrunGame;
		[Net] public RoundManager RoundManager { get; private set; }

		public DeathrunGame()
		{
			if ( Host.IsClient )
				return;

			RoundManager = new RoundManager();
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
			RoundManager.Round.ClientJoined( client );
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
	}
}
