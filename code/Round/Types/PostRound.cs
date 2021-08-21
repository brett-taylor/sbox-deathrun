using Sandbox;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Round.Types
{
	public class PostRound : BaseRound
	{
		[ConVar.ReplicatedAttribute( "dr_round_post_length" )]
		public static float POST_ROUND_LENGTH { get; set; } = 3f;

		public override RoundTimeLimit TimeLimit => RoundTimeLimit.WithLimit( POST_ROUND_LENGTH );
		public override RoundType RoundType => RoundType.POST;
		public override RoundType NextRound => RoundType.PREPARE;
		public override string RoundStartEventName => DeathrunEvents.ROUND_POST_STARTED;
		public override string RoundCompletedEventName => DeathrunEvents.ROUND_POST_COMPLETED;

		public override void RoundStart() { }

		public override void RoundEnd() { }

		public override void RoundUpdate() { }
	}
}
