using Sandbox;

namespace SBoxDeathrun.Pawn.Camera
{
	public class RagdollCamera : Sandbox.Camera
	{
		private Entity FocusEntity { get; set; }
		private Vector3 FocusPoint { get; set; }

		public override void Activated()
		{
			base.Activated();

			FieldOfView = CurrentView.FieldOfView;
		}

		public void SetFocusEntity( Entity entity )
		{
			if ( FocusEntity.IsValid() )
				return;

			FocusEntity = entity;
			FocusPoint = entity.Position - GetViewOffset();
		}

		public override void Update()
		{
			if ( Local.Client == null || FocusEntity.IsValid() == false ) return;

			FocusPoint = Vector3.Lerp( FocusPoint, GetSpectatePoint(), Time.Delta * 40f );
			Pos = FocusPoint + GetViewOffset();
			Rot = Input.Rotation;
			FieldOfView = FieldOfView.LerpTo( 50, Time.Delta * 3.0f );
			Viewer = null;
		}

		private Vector3 GetSpectatePoint()
		{
			return FocusEntity.PhysicsGroup.Pos;
		}

		private Vector3 GetViewOffset()
		{
			if ( Local.Client == null )
				return Vector3.Zero;

			return Input.Rotation.Forward * (-130 * 1) + Vector3.Up * (20 * 1);
		}
	}
}
