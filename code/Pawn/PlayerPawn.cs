using Sandbox;
using SBoxDeathrun.Entities;
using SBoxDeathrun.Pawn.Controller;
using SBoxDeathrun.Weapon;
using SBoxDeathrun.Weapon.Types;

namespace SBoxDeathrun.Pawn
{
	public partial class PlayerPawn : BasePawn
	{
		private DamageInfo LastDamage { get; set; }

		public new PlayerPawnController Controller
		{
			get => base.Controller as PlayerPawnController;
			set => base.Controller = value;
		}

		public PlayerPawn()
		{
			Inventory = new Inventory( this );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			Controller = new PlayerPawnController();
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

			var team = DeathrunGame.Current.TeamManager.GetTeamForClient( GetClientOwner() );
			var randomSpawnPoint = DeathrunSpawnPoint.SpawnPointForTeam( team );
			Position = randomSpawnPoint.Position;
			Rotation = randomSpawnPoint.Rotation;
		}

		public override void Simulate( Client cl )
		{
			if ( LifeState == LifeState.Dead )
				return;

			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );

			SimulateActiveChild( cl, ActiveChild );

			if ( Input.Pressed( InputButton.View ) )
				Camera = Camera is ThirdPersonCamera ? new FirstPersonCamera() : new ThirdPersonCamera();
		}

		public override void OnKilled()
		{
			CreateRagdoll( Velocity, LastDamage.Flags, LastDamage.Position, LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );
			base.OnKilled();

			EnableDrawing = false;
			EnableAllCollisions = false;
		}

		protected override void TakeActualDamage( DamageInfo info )
		{
			LastDamage = info;
		}
	}
}
