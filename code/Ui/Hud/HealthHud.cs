using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace SBoxDeathrun.Ui.Hud
{
	public class HealthHud : Panel
	{
		private readonly Panel MainContent;
		private readonly Label HealthLabel;

		public HealthHud()
		{
			StyleSheet.Load( "Ui/Hud/HealthHud.scss" );

			MainContent = Add.Panel( "main-content" );
			HealthLabel = MainContent.Add.Label( "100" );
		}

		public override void Tick()
		{
			if ( Local.Pawn.LifeState != LifeState.Alive )
			{
				MainContent.Style.Display = DisplayMode.None;
				MainContent.Style.Dirty();
				return;
			}

			HealthLabel.Text = $"{Local.Pawn.Health:0}";
			MainContent.Style.Display = DisplayMode.Flex;
			MainContent.Style.Dirty();
		}
	}
}
