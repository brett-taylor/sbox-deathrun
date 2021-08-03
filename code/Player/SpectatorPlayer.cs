using System;
using System.Linq;
using Sandbox;

namespace SBoxDeathrun.Player
{
	public class SpectatorPlayer : Sandbox.Player
	{
		public override void Respawn()
		{
			var sc = new SpectatorCamera();
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
