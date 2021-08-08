using Sandbox;
using SBoxDeathrun.Pawn.Camera;

namespace SBoxDeathrun.Pawn
{
	public partial class FreeCameraPawn : BasePawn
	{
		[Net] private Entity RagdollCameraFocusEntity { get; set; }

		public void Respawn( Entity corpse )
		{
			Camera = new RagdollCamera();
			RagdollCameraFocusEntity = corpse;

			RespawnShared();
		}

		public override void Respawn()
		{
			CreateFreeCamera();
			RespawnShared();
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

			if ( Camera is RagdollCamera rc && RagdollCameraFocusEntity.IsValid() )
				rc.SetFocusEntity( RagdollCameraFocusEntity );
		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );

			if ( ShouldSwapToFreeCamera() )
				CreateFreeCamera();
		}

		private bool ShouldSwapToFreeCamera()
		{
			return (Camera is RagdollCamera && RagdollCameraFocusEntity.IsValid() == false) || HasRequestedMovement();
		}

		private static bool HasRequestedMovement()
		{
			return Input.Forward != 0 || Input.Left != 0 || Input.Pressed( InputButton.Attack1 ) || Input.Pressed( InputButton.Attack2 );
		}

		private void CreateFreeCamera()
		{
			var sc = new FreeCamera();
			Camera = sc;
			sc.TargetPos = Position;
		}
	}
}
