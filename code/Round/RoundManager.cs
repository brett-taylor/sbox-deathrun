using System;
using Sandbox;
using SBoxDeathrun.Round.Types;
using SBoxDeathrun.Utils;

namespace SBoxDeathrun.Round
{
	public partial class RoundManager : Entity
	{
		[ConVar.ReplicatedAttribute( "deathrun_minimum_players" )]
		public static int MINIMUM_PLAYERS { get; set; } = 2;
		
		public BaseRound Round { get; private set; }
		[Net] public float RoundStartTime { get; private set; }
		[Net] public ActiveRoundOutcome LastActiveRoundOutcome { get; private set; }
		[Net, OnChangedCallback] private RoundType CurrentRoundType { get; set; }

		public RoundManager()
		{
			Transmit = TransmitType.Always;
			Round = new WaitingForPlayersRound();
		}

		public void Update()
		{
			Host.AssertServer();

			if ( Round is null )
				return;

			if ( CurrentRoundType != RoundType.WAITING_FOR_PLAYERS && Client.All.Count < MINIMUM_PLAYERS )
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
			Host.AssertServer();

			if ( Round is not null )
			{
				Round.RoundEnd();
				RunRoundEvent( DeathrunEvents.ROUND_ANY_COMPLETED, Round.RoundCompletedEventName );
			}

			var newRound = newRoundType.ToRound();
			RoundStartTime = Time.Now;
			newRound.RoundStart();
			Round = newRound;
			CurrentRoundType = newRoundType;

			RunRoundEvent( DeathrunEvents.ROUND_ANY_STARTED, Round.RoundStartEventName );
		}

		public void OnCurrentRoundTypeChanged()
		{
			Round = CurrentRoundType.ToRound();
		}

		private void RunRoundEvent( string generalRoundEventName, string eventName )
		{
			Event.Run( generalRoundEventName );
			RunRoundEventOnClient( generalRoundEventName );

			Event.Run( eventName );
			RunRoundEventOnClient( eventName );
		}

		[ClientRpc]
		private void RunRoundEventOnClient( string eventName )
		{
			Event.Run( eventName );
		}

		internal void SetLastActiveRoundOutcome( ActiveRoundOutcome outcome )
		{
			Host.AssertServer();

			if ( CurrentRoundType != RoundType.ACTIVE )
				throw new Exception( "SetLastActiveRoundOutcome invoked outside Active round type" );

			LastActiveRoundOutcome = outcome;
		}

		[ServerCmd( "deathrun_end_round" )]
		public static void EndRound()
		{
			DeathrunGame.Current.RoundManager.ChangeRounds(DeathrunGame.Current.RoundManager.Round.NextRound);
		}
	}
}
