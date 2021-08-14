using Sandbox;
using SBoxDeathrun.Entities;
using SBoxDeathrun.Pawn.Camera;

namespace SBoxDeathrun.Pawn
{
	public partial class FreeCameraPawn : BasePawn
	{
		[Net] private Entity RagdollCameraFocusEntity { get; set; }
		public Vector3 LastCurrentViewPosition { get; set; } = Vector3.Zero;
		public Rotation LastCurrentViewRotation { get; set; } = Rotation.Identity;

		public void Respawn( Entity corpse )
		{
			if ( corpse.IsValid() == false )
			{
				Respawn();
				return;
			}

			Camera = new RagdollCamera();
			RagdollCameraFocusEntity = corpse;

			RespawnShared();
		}

		public override void Respawn()
		{
			Camera = new FreeCamera();
			var isp = DeathrunInitialSpectatorPoint.Random();
			SetFreeCameraPositionAndRotation( To.Single( GetClientOwner() ), isp.Position, isp.Rotation );
			RespawnShared();
		}

		[ClientRpc]
		private void SetFreeCameraPositionAndRotation( Vector3 position, Rotation rotation )
		{
			if ( Camera is FreeCamera fc )
			{
				fc.TargetPos = position;
				fc.TargetRot = rotation;
			}
		}

		private void RespawnShared()
		{
			EnableAllCollisions = false;
			EnableDrawing = false;

			base.Respawn();
			LifeState = LifeState.Dead;
		}

		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );

			if ( Camera is RagdollCamera rc )
			{
				ClientVectorToServer( CurrentView.Position, CurrentView.Rotation );

				if ( RagdollCameraFocusEntity.IsValid() )
					rc.SetFocusEntity( RagdollCameraFocusEntity );
			}
		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );

			if ( ShouldSwapToFreeCamera() )
			{
				var fc = new FreeCamera();
				SetFreeCameraPositionAndRotation( To.Single( GetClientOwner() ), LastCurrentViewPosition, LastCurrentViewRotation );
				Camera = fc;
			}
		}

		[ServerCmd]
		public static void ClientVectorToServer( Vector3 currentViewPosition, Rotation currentViewRotation )
		{
			if ( ConsoleSystem.Caller.Pawn is FreeCameraPawn fcp )
			{
				fcp.LastCurrentViewPosition = currentViewPosition;
				fcp.LastCurrentViewRotation = currentViewRotation;
			}
		}

		private bool ShouldSwapToFreeCamera()
		{
			return (Camera is RagdollCamera && RagdollCameraFocusEntity.IsValid() == false) || HasRequestedMovement();
		}

		private static bool HasRequestedMovement()
		{
			return Input.Forward != 0 || Input.Left != 0 || Input.Pressed( InputButton.Attack1 ) || Input.Pressed( InputButton.Attack2 );
		}
	}
}
