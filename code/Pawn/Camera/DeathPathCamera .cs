using Sandbox;

namespace SBoxDeathrun.Pawn.Camera
{
	public class DeathPathCamera : Sandbox.Camera
	{
		[ConVar.ReplicatedAttribute( "deathrun_death_camera_target_fov" )]
		public static float TARGET_FOV { get; set; } = 80f;

		[ConVar.ReplicatedAttribute( "deathrun_death_camera_pitch_lower_bound" )]
		public static float PITCH_LOWER_BOUND { get; set; } = 20f;

		[ConVar.ReplicatedAttribute( "deathrun_death_camera_pitch_upper_bound" )]
		public static float PITCH_UPPER_BOUND { get; set; } = 70f;

		public Vector3 TargetPosition { get; set; }
		public Rotation TargetRotation { get; set; }
		private float YawAddition { get; set; }
		private float PitchAddition { get; set; }

		public override void Activated()
		{
			base.Activated();

			PitchAddition = PITCH_LOWER_BOUND.LerpTo( PITCH_UPPER_BOUND, 0.5f );
		}
		
		public override void Update()
		{
			Pos = TargetPosition;

			var angles = TargetRotation.Angles();
			angles.yaw += YawAddition;
			angles.pitch += PitchAddition;
			Rot = Rotation.From( angles );

			FieldOfView = TARGET_FOV;
		}

		public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );

			if ( input.Down( InputButton.Attack1 ) )
			{
				YawAddition += input.AnalogLook.yaw * (TARGET_FOV / 80.0f);
				PitchAddition += input.AnalogLook.pitch * (TARGET_FOV / 80.0f);
			}

			PitchAddition = PitchAddition.Clamp( PITCH_LOWER_BOUND, PITCH_UPPER_BOUND );
		}
	}
}
