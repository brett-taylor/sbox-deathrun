using Sandbox;

namespace SBoxDeathrun.Entities
{
	[Library( "deathrun_sbox_death_camera_path" )]
	public class DeathCameraPath : BasePathEntity
	{
		[ConVar.ReplicatedAttribute( "dr_camera_debug" )]
		public static bool DEATH_CAMERA_DEBUG { get; set; } = false;

		[Event.Tick.ServerAttribute]
		public void DebugDrawLine()
		{
			if ( DEATH_CAMERA_DEBUG )
				DrawPath( 20 );
		}
	}
}
