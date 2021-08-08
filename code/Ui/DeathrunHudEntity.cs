using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Ui.Hud;

namespace SBoxDeathrun.Ui
{
	public class DeathrunHudEntity : HudEntity<RootPanel>
	{
		private HealthHud healthHud;

		public DeathrunHudEntity()
		{
			if ( Host.IsClient == false )
				return;

			RootPanel.AddChild<RoundHud>();
			healthHud = RootPanel.AddChild<HealthHud>();

			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<CrosshairCanvas>();
			RootPanel.AddChild<KillFeed>();
		}

		[Event.HotloadAttribute]
		private void OnHotReload()
		{
			if ( healthHud is not null )
				healthHud.Delete( true );

			healthHud = RootPanel.AddChild<HealthHud>();
		}
	}
}
