using Sandbox;
using SBoxDeathrun.Round.Types;

namespace SBoxDeathrun.Round
{
	public partial class RoundManager : NetworkComponent
	{
		public Round Round { get; private set; }
		[Net] public float RoundStartTime { get; private set; }
		[Net, OnChangedCallback] public RoundType CurrentRoundType { get; private set; }

		public RoundManager()
		{
			Round = new WaitingForPlayersRound();
		}

		public void Update()
		{
			if ( Round is null )
				return;
			
			if ( CurrentRoundType != RoundType.WAITING_FOR_PLAYERS && Client.All.Count < RoundConfig.MINIMUM_PLAYERS )
			{
				ChangeRounds( RoundType.WAITING_FOR_PLAYERS );
				return;
			}

			Round.RoundUpdate();

			if ( Round.TimeLimit.HasLimit && Time.Now > RoundStartTime + Round.TimeLimit.Limit )
				Round.ShouldEnd = true;

			if ( Round.ShouldEnd )
				ChangeRounds( Round.NextRound );
		}

		private void ChangeRounds( RoundType newRoundType )
		{
			if ( Round is not null )
				Round.RoundEnd();

			var newRound = newRoundType.ToRound();
			RoundStartTime = Time.Now;
			newRound.RoundStart();
			Round = newRound;
			CurrentRoundType = newRoundType;
		}

		public void OnCurrentRoundTypeChanged()
		{
			Round = CurrentRoundType.ToRound();
		}
	}
}
