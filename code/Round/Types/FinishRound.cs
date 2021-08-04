namespace SBoxDeathrun.Round.Types
{
	public class FinishRound : Round
	{
		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( GameConfig.FINISH_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.FINISH;
		public override RoundType NextRound => RoundType.PREPARE;

		public override void RoundStart() { }
		
		public override void RoundEnd() { }
		
		public override void RoundUpdate() { }
	}
}
