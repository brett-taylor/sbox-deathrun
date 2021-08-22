using System;
using System.Linq;
using Sandbox;

namespace SBoxDeathrun.Entities.Paths
{
	public partial class DeathCameraPath
	{
		[ServerVar( "dr_death_camera_path_debug" )]
		public static bool DEATH_CAMERA_PATH_DEBUG { get; set; } = false;

		[Event.Entity.PostSpawnAttribute]
		private void CheckOnlyOneExists()
		{
			if ( All.OfType<DeathCameraPath>().Count() != 1 )
				throw new Exception( "Expect only one DeathCameraPath" );
		}

		private void AssertValidCurrentPathPercentage( float currentPathPercentage )
		{
			if ( currentPathPercentage < 0f || currentPathPercentage > 1f )
				throw new Exception( $"Invalid CurrentPathPercentage {currentPathPercentage}" );
		}

		[Event.Tick.ServerAttribute]
		public void DebugDrawLine()
		{
			if ( DEATH_CAMERA_PATH_DEBUG )
			{
				DrawPath( DEATH_CAMERA_PATH_SEGMENTS );
				DebugOverlay.Sphere(GetPositionOnCurve(0.25f), 5f, Color.Red);
				DebugOverlay.Sphere(GetPositionOnCurve(0.50f), 5f, Color.Red);
				DebugOverlay.Sphere(GetPositionOnCurve(0.75f), 5f, Color.Red);
				DebugOverlay.Sphere(GetPositionOnCurve(0.10f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.20f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.30f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.40f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.50f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.60f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.70f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.80f), 5f, Color.Blue);
				DebugOverlay.Sphere(GetPositionOnCurve(0.90f), 5f, Color.Blue);

				var lineIndex = 0;
				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, "DeathCameraPath Debug:" );
				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, $"Node Count: {PathNodes.Count}" );
				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, $"Segment Count: {Segments.Count}" );
				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, $"Path Length: {TotalLength}" );
				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, $"100% Percentage: {TotalPercentageFromSegments}" );

				lineIndex++;
				foreach ( var segment in Segments )
					DebugOverlay.ScreenText( new Vector2( Vector2.One * 100f ), lineIndex++, Color.Yellow, $"" +
						$"Segment: {segment.Length} Range: {segment.PercentageFrom} - {segment.PercentageTo}" );

				DebugOverlay.ScreenText( Vector2.One * 100f, lineIndex++, Color.Yellow, $"Length From Segments: {Segments.Select( s => s.Length ).Sum()}" );
			}
		}
	}
}
