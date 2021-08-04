using Sandbox;

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
			CreatePlayerPawn( client );
		}

		public override void RoundStart()
		{
			foreach ( var client in Client.All )
				CreatePlayerPawn( client );
		}

		public override void RoundEnd() { }

		public override void RoundUpdate() { }
	}
}
