using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Ui.Hud;

namespace SBoxDeathrun.Ui
{
	public class DeathrunHudEntity : HudEntity<RootPanel>
	{
		private DeathrunHud deathrunHud;

		public DeathrunHudEntity()
		{
			if ( Host.IsClient == false )
				return;

			deathrunHud = RootPanel.AddChild<DeathrunHud>();

			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		}

		[Event.HotloadAttribute]
		public void OnHotReload()
		{
			if ( Host.IsClient == false )
				return;

			if ( deathrunHud is not null )
				deathrunHud.Delete( true );
			deathrunHud = RootPanel.AddChild<DeathrunHud>();
		}
	}
}
