using System;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Pawn.Camera;

namespace SBoxDeathrun.Pawn
{
	public class FreeCameraPawn : BasePawn
	{
		public override void Respawn()
		{
			var sc = new FreeCamera();
			var spawnPoint = All
				.OfType<SpawnPoint>()
				.OrderBy( _ => Guid.NewGuid() )
				.FirstOrDefault();

			sc.TargetPos = spawnPoint.Position;
			Camera = sc;

			EnableAllCollisions = false;
			EnableDrawing = false;

			base.Respawn();
			LifeState = LifeState.Dead;
		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );
		}
	}
}
