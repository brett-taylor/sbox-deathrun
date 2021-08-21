using Sandbox;

namespace SBoxDeathrun.Pawn.Camera
{
	public class DeathPathCamera : Sandbox.Camera
	{
		[ConVar.ReplicatedAttribute( "dr_death_camera_target_fov" )]
		public static float TARGET_FOV { get; set; } = 80f;

		[ConVar.ReplicatedAttribute( "dr_death_camera_pitch_lower_bound" )]
		public static float PITCH_LOWER_BOUND { get; set; } = 20f;

		[ConVar.ReplicatedAttribute( "dr_death_camera_pitch_upper_bound" )]
		public static float PITCH_UPPER_BOUND { get; set; } = 70f;

		public Vector3 TargetPosition { get; set; }
		public Rotation TargetRotation { get; set; }
		private float InitialYaw { get; set; }
		private float YawOverride { get; set; }
		private float PitchOverride { get; set; }

		public override void Activated()
		{
			base.Activated();

			Rot = new Angles(
				PitchOverride,
				InitialYaw,
				0f
			).ToRotation();
		}

		public void SetInitialAngle( float initialYaw )
		{
			InitialYaw = initialYaw;
			YawOverride = InitialYaw;
			PitchOverride = PITCH_LOWER_BOUND.LerpTo( PITCH_UPPER_BOUND, 0.5f );

			Rot = new Angles(
				PitchOverride,
				InitialYaw,
				0f
			).ToRotation();
		}

		public override void Update()
		{
			Pos = TargetPosition;

			var angles = TargetRotation.Angles();
			angles.yaw = YawOverride;
			angles.pitch = PitchOverride;
			Rot = Rotation.From( angles );

			FieldOfView = TARGET_FOV;
		}

		public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );

			if ( input.Down( InputButton.Attack1 ) )
			{
				YawOverride += input.AnalogLook.yaw * (TARGET_FOV / 80.0f);
				PitchOverride += input.AnalogLook.pitch * (TARGET_FOV / 80.0f);
			}

			PitchOverride = PitchOverride.Clamp( PITCH_LOWER_BOUND, PITCH_UPPER_BOUND );
		}
	}
}
