using Sandbox;

namespace SBoxDeathrun.Pawn.Controller
{
	public class PlayerPawnController : WalkController
	{
		public PlayerPawnController()
		{
			AutoJump = true;
		}

		public override void Simulate()
		{
			if ( DeathrunGame.Current.RoundManager.Round.PawnsFrozen )
			{
				Input.Forward = 0;
				Input.Left = 0;
				Input.Up = 0;
			}

			base.Simulate();
		}
	}
}
