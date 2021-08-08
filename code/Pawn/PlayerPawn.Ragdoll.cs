using Sandbox;

namespace SBoxDeathrun.Pawn
{
	partial class PlayerPawn
	{
		private void CreateRagdoll( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int bone )
		{
			Host.AssertServer();

			var ragdoll = new ModelEntity();
			ragdoll.Position = Position;
			ragdoll.Rotation = Rotation;
			ragdoll.Scale = Scale;
			ragdoll.MoveType = MoveType.Physics;
			ragdoll.UsePhysicsCollision = true;
			ragdoll.EnableAllCollisions = true;
			ragdoll.SetModel( GetModelName() );
			ragdoll.CopyBonesFrom( this );
			ragdoll.CopyBodyGroups( this );
			ragdoll.CopyMaterialGroup( this );
			ragdoll.TakeDecalsFrom( this );
			ragdoll.EnableHitboxes = true;
			ragdoll.EnableAllCollisions = true;
			ragdoll.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ragdoll.RenderColorAndAlpha = RenderColorAndAlpha;
			ragdoll.PhysicsGroup.Velocity = velocity;
			ragdoll.CollisionGroup = CollisionGroup.Default;

			foreach ( var child in Children )
			{
				if ( child is ModelEntity e )
				{
					var model = e.GetModelName();
					if ( model != null && !model.Contains( "clothes" ) )
						continue;

					var clothing = new ModelEntity();
					clothing.SetModel( model );
					clothing.SetParent( ragdoll, true );
					clothing.RenderColorAndAlpha = e.RenderColorAndAlpha;
				}
			}

			if ( damageFlags.HasFlag( DamageFlags.Bullet ) || damageFlags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				var body = bone > 0 ? ragdoll.GetBonePhysicsBody( bone ) : null;
				if ( body != null )
					body.ApplyImpulseAt( forcePos, force * body.Mass );
				else
					ragdoll.PhysicsGroup.ApplyImpulse( force );
			}

			if ( damageFlags.HasFlag( DamageFlags.Blast ) && ragdoll.PhysicsGroup != null )
			{
				ragdoll.PhysicsGroup.AddVelocity( (Position - (forcePos + Vector3.Down * 100.0f)).Normal * (force.Length * 0.2f) );
				var angularDir = (Rotation.FromYaw( 90 ) * force.WithZ( 0 ).Normal).Normal;
				ragdoll.PhysicsGroup.AddAngularVelocity( angularDir * (force.Length * 0.02f) );
			}

			Corpse = ragdoll;
			ragdoll.DeleteAsync( 10.0f ); // TODO: Swap to keeping track of these and remove during Post->Prepare instead of timed 
		}
	}
}
