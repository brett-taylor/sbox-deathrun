using Sandbox;

namespace SBoxDeathrun.Player
{
	public class DeathrunPlayer : Sandbox.Player
	{
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			SimulateActiveChild( cl, ActiveChild );
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
		}
	}
}
