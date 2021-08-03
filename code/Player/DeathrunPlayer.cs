using Sandbox;
using SBoxDeathrun.Weapon;

namespace SBoxDeathrun.Player
{
	public class DeathrunPlayer : Sandbox.Player
	{
		public DeathrunPlayer()
		{
			Inventory = new Inventory( this );
		}

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

			var pistol = new Pistol();
			Inventory.Add( pistol );
			Inventory.SetActive( pistol );

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			if ( LifeState == LifeState.Dead )
				return;

			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );

			if ( Input.Pressed( InputButton.View ) )
				Camera = Camera is not FirstPersonCamera ? new FirstPersonCamera() : new ThirdPersonCamera();
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
			EnableAllCollisions = false;
		}
	}
}
