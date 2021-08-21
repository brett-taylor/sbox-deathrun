using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Pawn;

namespace SBoxDeathrun.Ui.Hud
{
	public class DeathCursor : Panel
	{
		public override void Tick()
		{
			if ( Local.Client.Pawn is DeathPathCameraPawn && !Input.Down(InputButton.Attack1) )
			{
				ShowCursor();
				return;
			}

			HideCursor();
		}

		private void HideCursor()
		{
			Style.PointerEvents = "none";
			Style.Dirty();
		}

		private void ShowCursor()
		{
			Style.PointerEvents = "visible";
			Style.Dirty();
		}
	}
}
