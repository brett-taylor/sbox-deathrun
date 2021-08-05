using Sandbox;
using Sandbox.UI;
using SBoxDeathrun.Team;

namespace SBoxDeathrun.Ui.Hud
{
	public class DeathrunHud : Panel
	{
		private readonly Label roundLabel;
		private readonly Label timeLeftLabel;
		private readonly Label playerTypeLabel;
		private readonly Label healthLabel;
		private readonly Label lifeStateLabel;
		private readonly Label teamLabel;
		private readonly Label frozenLabel;
		private readonly Label velocityLabel;

		public DeathrunHud()
		{
			StyleSheet.Load( "Ui/Hud/DeathrunHud.scss" );

			var title = AddChild<Label>( "title" );
			title.Text = "Temporary Deathrun Hud";

			roundLabel = AddChild<Label>();
			timeLeftLabel = AddChild<Label>();
			playerTypeLabel = AddChild<Label>();
			healthLabel = AddChild<Label>();
			lifeStateLabel = AddChild<Label>();
			teamLabel = AddChild<Label>();
			frozenLabel = AddChild<Label>();
			velocityLabel = AddChild<Label>();
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
			lifeStateLabel.Text = $"LifeState: {Local.Pawn.LifeState}";
			healthLabel.Text = $"Health: {Local.Pawn.Health}";
			teamLabel.Text = $"Team: {DeathrunGame.Current.TeamManager.GetTeamForClient( Local.Client ).NiceName()}";
			frozenLabel.Text = $"Frozen Round: {currentRound.PawnsFrozen}";
			velocityLabel.Text = $"Frozen Round: {Local.Pawn.Velocity.Length}";
		}
	}
}
