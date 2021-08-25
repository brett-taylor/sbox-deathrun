using Hammer;
using Sandbox;
using SBoxDeathrun.Entities.Attributes;
using SBoxDeathrun.Entities.Paths;

namespace SBoxDeathrun.Entities.Triggers
{
	[Library( "deathrun_trap_camera" )]
	[EntityTool( "Deathrun Trap Camera", "Deathrun Sbox", "A camera for a trap" )]
	[EditorModel( "models/editor/camera.vmdl" )]
	[ShowFrustum( "80", "10", "80000", "Color" )]
	public class TrapCamera : Entity
	{
		[Property] private Color Color { get; set; } = Color.Blue;

		public float DeathPathLocation { get; private set; } = 0f;

		public void CalculateDeathPathLocation()
		{
			DeathPathLocation = DeathCameraPath.Get().GetPercentageFromPoint( Position );
		}
	}
}
