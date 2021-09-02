using Sandbox.UI;
using Sandbox.UI.Construct;

namespace SBoxDeathrun.Ui.Hud
{
	public class TrackLineUI : Panel
	{
		private readonly Panel container = null;
		private bool hasSetup = false;

		public TrackLineUI()
		{
			StyleSheet.Load( "Ui/Track/TrackLineUI.scss" );

			Add.Label( "Start", "block big-text" );

			container = Add.Panel( "main-contents" );
			var middleLineContainer = container.Add.Label( "b", "middle-line-container" );
			middleLineContainer.Add.Panel( "middle-line" );
			
			Add.Label( "End", "block big-text" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( hasSetup == true )
				return;

			if ( DeathrunGame.Current.TrapManager is null )
				return;

			foreach ( var trap in DeathrunGame.Current.TrapManager.Traps )
			{
				var tc = container.Add.Panel( "trap" );
				var tlt = tc.AddChild<TrackLineTrap>();
				tlt.Trap = trap;
			}

			hasSetup = true;
		}
	}
}
