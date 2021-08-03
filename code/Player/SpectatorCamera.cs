using System;
using Sandbox;

namespace SBoxDeathrun.Player
{
	public class SpectatorCamera : Camera
	{
		public Vector3 TargetPos { get; set; }
		public Rotation TargetRot { get; set; }

		private Angles LookAngles;
		private Vector3 MoveInput;
		private bool PivotEnabled;
		private Vector3 PivotPos;
		private float PivotDist;
		private float MoveSpeed;
		private float FovOverride = 0;
		private float LerpMode = 0;

		public override void Activated()
		{
			base.Activated();

			TargetPos = CurrentView.Position;
			TargetRot = CurrentView.Rotation;

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

			if ( PivotEnabled )
				PivotMove();
			else
				FreeMove();
		}

		public override void BuildInput( InputBuilder input )
		{
			MoveInput = input.AnalogMove;

			MoveSpeed = 1;
			if ( input.Down( InputButton.Run ) ) MoveSpeed = 5;
			if ( input.Down( InputButton.Duck ) ) MoveSpeed = 0.2f;

			if ( input.Down( InputButton.Slot1 ) ) LerpMode = 0.0f;
			if ( input.Down( InputButton.Slot2 ) ) LerpMode = 0.5f;
			if ( input.Down( InputButton.Slot3 ) ) LerpMode = 0.9f;

			if ( input.Pressed( InputButton.Walk ) )
			{
				var tr = Trace.Ray( Pos, Pos + Rot.Forward * 4096 ).Run();
				PivotPos = tr.EndPos;
				PivotDist = Vector3.DistanceBetween( tr.EndPos, Pos );
			}

			if ( input.Down( InputButton.Use ) )
				DoFBlurSize = Math.Clamp( DoFBlurSize + (Time.Delta * 3.0f), 0.0f, 100.0f );

			if ( input.Down( InputButton.Menu ) )
				DoFBlurSize = Math.Clamp( DoFBlurSize - (Time.Delta * 3.0f), 0.0f, 100.0f );

			if ( input.Down( InputButton.Attack2 ) )
			{
				FovOverride += input.AnalogLook.pitch * (FovOverride / 30.0f);
				FovOverride = FovOverride.Clamp( 5, 150 );
				input.AnalogLook = default;
			}

			LookAngles += input.AnalogLook * (FovOverride / 80.0f);
			LookAngles.roll = 0;

			PivotEnabled = input.Down( InputButton.Walk );

			input.Clear();
			input.StopProcessing = true;
		}

		private void FreeMove()
		{
			var mv = MoveInput.Normal * 300 * RealTime.Delta * Rot * MoveSpeed;

			TargetRot = Rotation.From( LookAngles );
			TargetPos += mv;

			Pos = Vector3.Lerp( Pos, TargetPos, 10 * RealTime.Delta * (1 - LerpMode) );
			Rot = Rotation.Slerp( Rot, TargetRot, 10 * RealTime.Delta * (1 - LerpMode) );
		}

		private void PivotMove()
		{
			PivotDist -= MoveInput.x * RealTime.Delta * 100 * (PivotDist / 50);
			PivotDist = PivotDist.Clamp( 1, 1000 );

			TargetRot = Rotation.From( LookAngles );
			Rot = Rotation.Slerp( Rot, TargetRot, 10 * RealTime.Delta * (1 - LerpMode) );

			TargetPos = PivotPos + Rot.Forward * -PivotDist;
			Pos = TargetPos;
		}
	}
}
