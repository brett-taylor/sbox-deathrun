using Sandbox;
using Sandbox.UI;

namespace SBoxDeathrun.Ui.Hud
{
	public class DeathrunHud : Panel
	{
		private readonly Label roundLabel;
		private readonly Label timeLeftLabel;
		private readonly Label playerTypeLabel;

		public DeathrunHud()
		{
			StyleSheet.Load( "Ui/Hud/DeathrunHud.scss" );

			var title = AddChild<Label>( "title" );
			title.Text = "Temporary Deathrun Hud";

			roundLabel = AddChild<Label>();
			timeLeftLabel = AddChild<Label>();
			playerTypeLabel = AddChild<Label>();
		}

		public override void Tick()
		{
			base.Tick();

			var rm = DeathrunGame.Current.RoundManager;
			var currentRound = rm.Round;

			roundLabel.Text = $"Round: {currentRound.RoundType.ToString()}";
			timeLeftLabel.Text = currentRound.TimeLimit.HasLimit
				? $"Time Left: {(rm.RoundStartTime + currentRound.TimeLimit.Limit) - Time.Now}"
				: "Time Left: Limitless";
			playerTypeLabel.Text = $"Player Type: {Local.Pawn}";
		}
	}
}
