using Sandbox;
using Sandbox.UI;

namespace SBoxDeathrun.Ui.Hud
{
	public class DeathrunHud : Panel
	{
		private readonly Label roundLabel;
		private readonly Label timeLeftLabel;
		private readonly Label playerTypeLabel;
		private readonly Label healthLabel;
		private readonly Label teamLabel;
		private readonly Label frozenLabel;

		public DeathrunHud()
		{
			StyleSheet.Load( "Ui/Hud/DeathrunHud.scss" );

			var title = AddChild<Label>( "title" );
			title.Text = "Temporary Deathrun Hud";

			roundLabel = AddChild<Label>();
			timeLeftLabel = AddChild<Label>();
			playerTypeLabel = AddChild<Label>();
			healthLabel = AddChild<Label>();
			teamLabel = AddChild<Label>();
			frozenLabel = AddChild<Label>();
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
			healthLabel.Text = $"Health: {Local.Pawn.Health}";
			teamLabel.Text = $"Team: No team";
			frozenLabel.Text = $"Frozen Round: {currentRound.PawnsFrozen}";
		}
	}
}
