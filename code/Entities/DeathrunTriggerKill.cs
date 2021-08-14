using Sandbox;

namespace SBoxDeathrun.Entities.Map
{
	[Library( "deathrun_sbox_trigger_kill" )]
	public partial class DeathrunTriggerKill : BaseTrigger
	{
		public override void OnTouchStartAll( Entity toucher )
		{
			base.OnTouchStartAll( toucher );
			toucher.TakeDamage( DamageInfo.Generic( toucher.Health ) );
		}
	}
}
