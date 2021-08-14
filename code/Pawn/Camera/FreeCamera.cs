using Sandbox;

namespace SBoxDeathrun.Pawn.Camera
{
	public class FreeCamera : Sandbox.Camera
	{
		public Vector3 TargetPos { get; set; }
		public Rotation TargetRot { get; set; }

		private Angles LookAngles;
		private Vector3 MoveInput;
		private float MoveSpeed;
		private float FovOverride = 0;

		public override void Activated()
		{
			base.Activated();

			Pos = TargetPos;
			Rot = TargetRot;
			LookAngles = Rot.Angles();
			FovOverride = 80;

			DoFPoint = 0.0f;
			DoFBlurSize = 0.0f;
		}

		public override void Update()
		{
			var player = Local.Client;
			if ( player == null )
				return;

			var tr = Trace.Ray( Pos, Pos + Rot.Forward * 4096 ).UseHitboxes().Run();
			FieldOfView = FovOverride;

			Viewer = null;
			{
				var lerpTarget = tr.EndPos.Distance( Pos );
				DoFPoint = lerpTarget;
			}

			FreeMove();
		}

		public override void BuildInput( InputBuilder input )
		{
			MoveInput = input.AnalogMove;
			MoveSpeed = 1;
			if ( input.Down( InputButton.Run ) ) MoveSpeed = 5;
			if ( input.Down( InputButton.Duck ) ) MoveSpeed = 0.2f;

			LookAngles += input.AnalogLook * (FovOverride / 80.0f);
			LookAngles.roll = 0;

			input.Clear();
			input.StopProcessing = true;
		}

		private void FreeMove()
		{
			var mv = MoveInput.Normal * 300 * RealTime.Delta * Rot * MoveSpeed;

			TargetRot = Rotation.From( LookAngles );
			TargetPos += mv;

			Pos = Vector3.Lerp( Pos, TargetPos, 10 * RealTime.Delta );
			Rot = Rotation.Slerp( Rot, TargetRot, 10 * RealTime.Delta );
		}
	}
}
