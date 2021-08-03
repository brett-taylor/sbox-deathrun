namespace SBoxDeathrun.Round.Types
{
	public class ActiveRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( RoundConfig.ACTIVE_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.ACTIVE;
		public override RoundType NextRound => RoundType.FINISH;

		public override void RoundStart() { }
		
		public override void RoundEnd() { }
		
		public override void RoundUpdate() { }
	}
}
