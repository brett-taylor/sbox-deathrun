using Sandbox;
using SBoxDeathrun.Entities.Paths;
using SBoxDeathrun.Pawn.Camera;

namespace SBoxDeathrun.Pawn
{
	public partial class DeathPathCameraPawn : BasePawn
	{
		[ConVar.ReplicatedAttribute( "deathrun_death_camera_pawn_move_speed" )]
		public static float MOVE_SPEED { get; set; } = 0.2f;
		
		[Net, Local] private Vector3 TargetCameraPosition { get; set; }
		[Net, Local] private Rotation TargetCameraRotation { get; set; }

		private DeathCameraPath DeathCameraPath { get; set; }
		private float DeathPathPercentage { get; set; }

		public override void Respawn()
		{
			DeathCameraPath = DeathCameraPath.Get();
			DeathPathPercentage = 0f;

			Camera = new DeathPathCamera();

			EnableAllCollisions = false;
			EnableDrawing = false;

			base.Respawn();
			LifeState = LifeState.Alive;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( IsServer )
			{
				DeathPathPercentage += Input.Down( InputButton.Forward ) ? MOVE_SPEED * Time.Delta : 0f;
				DeathPathPercentage += Input.Down( InputButton.Back ) ? -MOVE_SPEED * Time.Delta : 0f;
				DeathPathPercentage = DeathPathPercentage.Clamp( 0f, 1f );
				var PositionAndRotation = DeathCameraPath.GetPositionAndRotationOnCurve( DeathPathPercentage );
				TargetCameraPosition = PositionAndRotation.position;
				TargetCameraRotation = PositionAndRotation.rotation;
			}

			var dpc = Camera as DeathPathCamera;
			dpc.TargetPosition = TargetCameraPosition;
			dpc.TargetRotation = TargetCameraRotation;
		}
	}
}
