using Sandbox;

namespace SBoxDeathrun.Player.Controller
{
	public class AlivePlayerController : WalkController
	{
		public AlivePlayerController()
		{
			AutoJump = true;
		}

		public override void Simulate()
		{
			if ( DeathrunGame.Current.RoundManager.Round.PlayersFrozen )
			{
				Input.Forward = 0;
				Input.Left = 0;
				Input.Up = 0;
			}

			base.Simulate();
		}
	}
}
