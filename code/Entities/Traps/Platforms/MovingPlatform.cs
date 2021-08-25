using Sandbox;
using System;
using System.Threading.Tasks;

namespace SBoxDeathrun.Entities.Traps.Platforms
{
	[Library("deathrun_moving_platform")]
	[Hammer.SupportsSolid]
	[Hammer.Model]
	[Hammer.RenderFields]
	[Hammer.DrawAngles( "movedir", "movedir_islocal" )]
	public partial class MovingPlatform : ModelEntity
	{
		[Property( "movedir", Title = "Move Direction" )]
		public Angles MoveDir { get; set; } = new Angles( 0, 0, 0 );

		[Property( "movedir_islocal", Title = "Move Direction is Expressed in Local Space" )]
		public bool MoveDirIsLocal { get; set; } = true;

		public enum PlatformMoveType
		{
			//Moving = 0 // Moving with Distance=Lip
			Moving = 3, // StartPos + Dir * Distance
			Rotating = 1,
			RotatingContinious = 4, // Rotating without reversing
			//NotMoving = 2,
		}

		[Property( "movedir_type", Title = "Movement Type" )]
		public PlatformMoveType MoveDirType { get; set; } = PlatformMoveType.Moving;

		[Property] public float MoveDistance { get; set; } = 100.0f;
		[Property] protected float Speed { get; set; } = 64.0f;
		[Property] protected float TimeToHold { get; set; } = 0.0f;

		[Flags]	public enum Flags
		{
			StartsMoving = 1,
			LoopMovement = 2,
		}

		[Property( "spawnsettings", Title = "Spawn Settings", FGDType = "flags" )]
		public Flags SpawnSettings { get; set; } = Flags.StartsMoving;

		[Property( FGDType = "sound", Group = "Sounds" )]
		public string StartMoveSound { get; set; }

		[Property( FGDType = "sound", Group = "Sounds" )]
		public string StopMoveSound { get; set; }

		[Property( "moving_sound", FGDType = "sound", Group = "Sounds" )]
		public string MovingSound { get; set; }

		public bool IsMoving { get; protected set; }
		public bool IsMovingForwards { get; protected set; }

		Vector3 PositionA;
		Vector3 PositionB;
		Rotation RotationA;
		public override void Spawn()
		{
			base.Spawn();

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
			SetInteractsExclude( CollisionLayer.All );
			PhysicsBody.GravityEnabled = false;

			// PlatformMoveType.Moving
			{
				PositionA = LocalPosition;
				PositionB = PositionA + MoveDir.Direction * MoveDistance;

				if ( MoveDirIsLocal )
				{
					var dir_world = Transform.NormalToWorld( MoveDir.Direction );
					PositionB = PositionA + dir_world * MoveDistance;
				}
			}

			// DoorMoveType.Rotating
			{
				RotationA = LocalRotation;
			}

			IsMoving = false;
			IsMovingForwards = true;

			if ( SpawnSettings.HasFlag( Flags.StartsMoving ) )
			{
				StartMoving();
			}
		}

		protected override void OnDestroy()
		{
			if ( MoveSoundInstance.HasValue )
			{
				MoveSoundInstance.Value.Stop();
				MoveSoundInstance = null;
			}

			base.OnDestroy();
		}

		protected Output OnReachedStart { get; set; }
		protected Output OnReachedEnd { get; set; }

		int movement = 0;
		Sound? MoveSoundInstance = null;
		float currentRotation = 0;
		async Task DoMove()
		{
			if ( !IsMoving ) Sound.FromEntity( StartMoveSound, this );

			IsMoving = true;
			var moveId = ++movement;

			if ( !MoveSoundInstance.HasValue && !string.IsNullOrEmpty( MovingSound ) )
			{
				MoveSoundInstance = PlaySound( MovingSound );
			}

			if ( MoveDirType == PlatformMoveType.Moving )
			{
				var position = IsMovingForwards ? PositionB : PositionA;

				var distance = Vector3.DistanceBetween( LocalPosition, position );
				var timeToTake = distance / Math.Max( Speed, 0.1f );

				var success = await LocalKeyframeTo( position, timeToTake, null );
				if ( !success )
					return;
			}
			else if ( MoveDirType == PlatformMoveType.RotatingContinious || MoveDirType == PlatformMoveType.Rotating )
			{
				var moveDist = MoveDistance;
				if ( IsMovingForwards ) moveDist = -moveDist;

				var timeToTake = Math.Abs( moveDist ) / Math.Max( Speed, 0.1f );
				var initialRotation = currentRotation - (currentRotation % moveDist);

				for ( float f = currentRotation % moveDist / moveDist; f < 1; )
				{
					await Task.NextPhysicsFrame();

					if ( moveId != movement || !this.IsValid() ) return;

					var axis = Rotation.From( MoveDir ).Up;
					if ( !MoveDirIsLocal ) axis = Transform.NormalToLocal( axis );

					var delta = Time.Delta / timeToTake;
					currentRotation += delta * moveDist;
					LocalRotation = RotationA.RotateAroundAxis( axis, currentRotation );
					f += delta;
				}

				// Snap to the ideal final position
				var axis_f = Rotation.From( MoveDir ).Up;
				if ( !MoveDirIsLocal ) axis_f = Transform.NormalToLocal( axis_f );
				currentRotation = initialRotation + moveDist;
				LocalRotation = RotationA.RotateAroundAxis( axis_f, currentRotation );
			}
			else { Log.Warning( $"{this}: Unknown platform move type {MoveDirType}!" ); await Task.Delay( 100 ); }

			if ( moveId != movement || !this.IsValid() ) return;

			if ( IsMovingForwards )
			{
				_ = OnReachedEnd.Fire( this );
			}
			else
			{
				_ = OnReachedStart.Fire( this );
			}

			if ( MoveDirType != PlatformMoveType.RotatingContinious || TimeToHold > 0 )
			{
				IsMoving = false;

				if ( MoveSoundInstance.HasValue )
				{
					MoveSoundInstance.Value.Stop();
					MoveSoundInstance = null;
				}

				// Do not play the stop sound for instant changing direction
				if ( !SpawnSettings.HasFlag( Flags.LoopMovement ) || TimeToHold > 0 )
				{
					Sound.FromEntity( StopMoveSound, this );
				}
			}

			if ( !SpawnSettings.HasFlag( Flags.LoopMovement ) ) return;
			if ( TimeToHold > 0 ) await Task.DelaySeconds( TimeToHold );
			if ( moveId != movement || !this.IsValid() ) return;
			if ( MoveDirType != PlatformMoveType.RotatingContinious )
				IsMovingForwards ^= true;

			_ = DoMove();
		}

		[Input] public void StartMoving() => _ = DoMove();
		[Input] public void StartMovingForward()
		{
			IsMovingForwards = true;
			_ = DoMove();
		}
		[Input] public void StartMovingBackwards()
		{
			IsMovingForwards = false;
			_ = DoMove();
		}

		[Input] public void ReverseMoving()
		{
			IsMovingForwards ^= true;

			if ( IsMoving )
			{
				// We changed direction, play the start moving sound
				Sound.FromEntity( StartMoveSound, this );
				_ = DoMove();
			}
		}

		[Input] public void StopMoving()
		{
			if ( !IsMoving ) return;

			movement++;
			_ = LocalKeyframeTo( LocalPosition, 0, null ); // Bad
			_ = LocalRotateKeyframeTo( LocalRotation, 0, null );

			IsMoving = false;
			Sound.FromEntity( StopMoveSound, this );

			if ( MoveSoundInstance.HasValue )
			{
				MoveSoundInstance.Value.Stop();
				MoveSoundInstance = null;
			}
		}

		private int local_movement = 0;
		private int local_rot_movement = 0;
		public async Task<bool> LocalKeyframeTo( Vector3 deltaTarget, float seconds, Easing.Function easing = null )
		{
			var moveid = ++local_movement;
			var startPos = LocalPosition;
			var movementVector = (deltaTarget - startPos) / seconds;

			for ( float f = 0; f < 1f - 0.001f; )
			{
				await Task.NextPhysicsFrame();

				if ( !this.IsValid() ) return false;
				if ( moveid != local_movement )
				{
					PhysicsBody.Velocity = Vector3.Zero;
					return false;
				}

				if ( easing == null ) PhysicsBody.Velocity = movementVector;
				else
				{
					var easedA = easing( f );
					var easedB = easing( f + 0.001f );

					var easeDistance = easedA - easedB;
					var ratio = 0.001f / easeDistance;
					PhysicsBody.Velocity = movementVector * ratio;
				}

				f += Time.Delta / seconds;
			}

			PhysicsBody.Velocity = Vector3.Zero;
			LocalPosition = deltaTarget;

			return true;
		}

		public async Task<bool> LocalRotateKeyframeTo( Rotation localTarget, float seconds, Easing.Function easing = null )
		{
			var moveId = ++local_rot_movement;
			var startPos = LocalRotation;
			var movementVector = (localTarget * startPos.Inverse).Angles().Direction / seconds;

			for ( float f = 0; f < 1; )
			{
				await Task.NextPhysicsFrame();

				if ( !this.IsValid() ) return false;
				if ( moveId != local_rot_movement )
				{
					PhysicsBody.AngularVelocity = Vector3.Zero;
					return false;
				}

				if ( easing == null ) PhysicsBody.AngularVelocity = movementVector;
				else
				{
					var easedA = easing( f );
					var easedB = easing( f + 0.001f );

					var easeDistance = easedA - easedB;
					var ratio = 0.001f / easeDistance;
					PhysicsBody.AngularVelocity = movementVector * ratio;
				}


				f += Time.Delta / seconds;
			}

			PhysicsBody.AngularVelocity = Vector3.Zero;
			LocalRotation = localTarget;

			return true;
		}

		[Input] public void ToggleMoving()
		{
			if ( IsMoving )
			{
				StopMoving();
				return;
			}

			StartMoving();
		}

		[Input] public void SetSpeed( float speed )
		{
			Speed = speed;

			// Update the moving speed
			if ( IsMoving ) _ = DoMove();
		}
	}
}
