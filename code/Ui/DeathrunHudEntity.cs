using System;
using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Ui.Hud;

namespace SBoxDeathrun.Ui
{
	public class DeathrunHudEntity : HudEntity<RootPanel>
	{
		public static DeathrunHudEntity Current { get; private set; }

		private TrackLineUI trackLineUi = null;

		public DeathrunHudEntity()
		{
			if ( Host.IsClient == false )
				return;

			if ( Current is not null )
				throw new Exception( $"DeathrunHudEntity already exists on client" );

			Current = this;

			RootPanel.AddChild<RoundHud>();
			RootPanel.AddChild<HealthHud>();
			RootPanel.AddChild<DeathCursor>();
			trackLineUi = RootPanel.AddChild<TrackLineUI>();

			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<CrosshairCanvas>();
			RootPanel.AddChild<KillFeed>();
		}

		[Event.HotloadAttribute]
		public void HotReload()
		{
			if ( trackLineUi is not null )
				trackLineUi.Delete( true );

			trackLineUi = RootPanel.AddChild<TrackLineUI>();
		}
	}
}
