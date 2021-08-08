using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SBoxDeathrun.Round;
using SBoxDeathrun.Team;
using SBoxDeathrun.Utils;
using SBoxDeathrun.Utils.Extensions;

namespace SBoxDeathrun.Ui.Hud
{
	public class RoundHud : Panel
	{
		private readonly Label roundStageLabel;
		private readonly Label helpLabel;
		private ActiveRoundOutcome lastOutcome;

		public RoundHud()
		{
			StyleSheet.Load( "Ui/Hud/RoundHud.scss" );

			var mainContent = Add.Panel( "main-content" );
			roundStageLabel = mainContent.Add.Label( "", "round-stage-label" );
			helpLabel = mainContent.Add.Label( "Sit back and watch the action", "help-label" );
		}

		public override void Tick()
		{
			base.Tick();

			var currentRound = DeathrunGame.Current.RoundManager.Round;
			roundStageLabel.Text = RoundStageText( currentRound );
			helpLabel.Text = HelpText( currentRound );
		}

		private static string RoundStageText( BaseRound currentRound )
		{
			var roundManager = DeathrunGame.Current.RoundManager;
			return (currentRound.RoundType) switch
			{
				RoundType.WAITING_FOR_PLAYERS => "Waiting For Players",
				RoundType.PREPARE => $"Prepare {RoundTimeRemaining( roundManager, currentRound )}",
				RoundType.ACTIVE => $"Run {RoundTimeRemaining( roundManager, currentRound )}",
				RoundType.POST => $"Relax {RoundTimeRemaining( roundManager, currentRound )}",
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private static string RoundTimeRemaining( RoundManager roundManager, BaseRound round )
		{
			Assert.True( round.TimeLimit.HasLimit );

			var timeEndSpan = TimeSpan.FromSeconds( (roundManager.RoundStartTime + round.TimeLimit.Limit) - Time.Now );
			var minutes = timeEndSpan.Minutes;
			var seconds = timeEndSpan.Seconds;
			return $"{minutes:D2}:{seconds:D2}";
		}

		private string HelpText( BaseRound currentRound )
		{
			var team = DeathrunGame.Current.TeamManager.GetTeamForClient( Local.Client );

			if ( currentRound.RoundType == RoundType.WAITING_FOR_PLAYERS )
			{
				var playersRequired = GameConfig.MINIMUM_PLAYERS - Client.All.Count;
				return $"{playersRequired} More {"Player".Plural( playersRequired )} {"Is".Plural( playersRequired, "Are" )} Required.";
			}

			return (currentRound.RoundType) switch
			{
				RoundType.PREPARE => $"You have been selected as Team {team.NiceName()}",
				RoundType.ACTIVE => HelpTextForActive( team ),
				RoundType.POST => HelpTextForPost(),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private static string HelpTextForActive( TeamType team )
		{
			if ( team == TeamType.SPECTATOR || Local.Client.Pawn.LifeState != LifeState.Alive )
				return "Sit Back And Relax";
			
			return team switch
			{
				TeamType.DEATH => $"Kill The Runners Before They Reach The End",
				TeamType.RUNNER => $"Don't Get Caught By Death",
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private string HelpTextForPost()
		{
			return (lastOutcome) switch
			{
				ActiveRoundOutcome.TIED => "Neither Team Wins",
				ActiveRoundOutcome.RUNNERS_WIN => "Team Runners Win",
				ActiveRoundOutcome.DEATHS_WIN => "Team Deaths Win",
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		
		[Event(DeathrunEvents.ROUND_ACTIVE_COMPLETED)]
		private void OnAnyRoundCompleted()
		{
			lastOutcome = DeathrunGame.Current.RoundManager.LastActiveRoundOutcome;
		}
	}
}
