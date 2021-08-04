using Sandbox;
using SBoxDeathrun.Pawn.Camera;

namespace SBoxDeathrun.Pawn
{
	public class FreeCameraPawn : BasePawn
	{
		public override void Respawn()
		{
			var sc = new FreeCamera();
			Camera = sc;

			EnableAllCollisions = false;
			EnableDrawing = false;

			base.Respawn();
			LifeState = LifeState.Dead;

			sc.TargetPos = Position;
		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );
		}
	}
}
