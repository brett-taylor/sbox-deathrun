using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Ui.Hud;

namespace SBoxDeathrun.Ui
{
	public class DeathrunHudEntity : HudEntity<RootPanel>
	{
		public DeathrunHudEntity()
		{
			if ( Host.IsClient == false )
				return;

			RootPanel.AddChild<RoundHud>();
			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<CrosshairCanvas>();
			RootPanel.AddChild<KillFeed>();
		}
	}
}
