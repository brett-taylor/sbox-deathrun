using Hammer;
using Sandbox;

namespace SBoxDeathrun.Entities.Triggers
{
	[Library( "deathrun_trigger_kill" )]
	[EntityTool( "Deathrun Trigger Kill", "Deathrun Sbox", "Instant kill trigger" )]
	public partial class InstantKillTrigger : BaseTrigger
	{
		public override void OnTouchStartAll( Entity toucher )
		{
			base.OnTouchStartAll( toucher );
			toucher.TakeDamage( DamageInfo.Generic( toucher.Health ) );
		}
	}
}
