using Sandbox.UI;
using Sandbox.UI.Construct;
using SBoxDeathrun.Entities.Triggers;

namespace SBoxDeathrun.Ui.Hud
{
	public class TrackLineTrap : Panel
	{
		public Trap Trap { get; set; }

		public TrackLineTrap()
		{
			StyleSheet.Load( "Ui/Track/TrackLineTrap.scss" );
			Add.Label( "blah" );
		}
	}
}
