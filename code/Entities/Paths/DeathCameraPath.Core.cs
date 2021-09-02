using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Utils.Extensions;

namespace SBoxDeathrun.Entities.Paths
{
	[Library( "deathrun_death_camera_path" )]
	public partial class DeathCameraPath : BasePathEntity
	{
		[ServerVar( "deathrun_death_camera_path_segments" )]
		public static int DEATH_CAMERA_PATH_SEGMENTS { get; set; } = 10;

		private float TotalPercentageFromSegments { get; set; }
		private float TotalLength { get; set; }
		private IReadOnlyList<DeathCameraPathSegment> Segments { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			CalculateTotalPathLength();
			CalculateSegments();
		}

		private void CalculateTotalPathLength()
		{
			var totalLength = 0f;
			for ( var i = 0; i < PathNodes.Count - 1; i++ )
			{
				totalLength += GetCurveLength( PathNodes[i], PathNodes[i + 1], DEATH_CAMERA_PATH_SEGMENTS );
			}

			TotalLength = totalLength;
		}

		private void CalculateSegments()
		{
			var list = new List<DeathCameraPathSegment>();
			var previousPercentage = 0f;
			for ( var i = 0; i < PathNodes.Count - 1; i++ )
			{
				var s = new DeathCameraPathSegment(
					PathNodes[i],
					PathNodes[i + 1],
					GetCurveLength( PathNodes[i], PathNodes[i + 1], DEATH_CAMERA_PATH_SEGMENTS ),
					TotalLength,
					previousPercentage
				);

				previousPercentage += s.Percentage;
				list.Add( s );
			}

			Segments = list;
			TotalPercentageFromSegments = Segments.Select( s => s.Percentage ).Sum();
		}

		private DeathCameraPathSegment GetSegmentFromPercentage( float currentPathPercentage )
		{
			AssertValidCurrentPathPercentage( currentPathPercentage );
			return Segments.First( s => s.IsPercentageWithinRange( currentPathPercentage, TotalPercentageFromSegments ) );
		}

		public (Vector3 position, Rotation rotation) GetPositionAndRotationOnCurve( float currentPathPercentage )
		{
			var currentSegment = GetSegmentFromPercentage( currentPathPercentage );
			var requiredDistance = TotalLength * currentPathPercentage;
			var segmentLowerDistance = TotalLength * currentSegment.PercentageFrom;
			var remainingDistance = requiredDistance - segmentLowerDistance;
			var finalAdjustedPercentage = remainingDistance / currentSegment.Length;
			var (point, normal) = this.GetPointAndNormalBetweenNodes( currentSegment.From, currentSegment.To, finalAdjustedPercentage );
			return (point, Rotation.LookAt( normal ));
		}

		public Vector3 GetPositionOnCurve( float currentPathPercentage )
		{
			var currentSegment = GetSegmentFromPercentage( currentPathPercentage );
			var requiredDistance = TotalLength * currentPathPercentage;
			var segmentLowerDistance = TotalLength * currentSegment.PercentageFrom;
			var remainingDistance = requiredDistance - segmentLowerDistance;
			var finalAdjustedPercentage = remainingDistance / currentSegment.Length;
			return GetPointBetweenNodes( currentSegment.From, currentSegment.To, finalAdjustedPercentage );
		}

		public static DeathCameraPath Get()
		{
			return All.OfType<DeathCameraPath>().First();
		}

		public float GetPercentageFromPoint( Vector3 point )
		{
			// 0.0025f accuracy is 9 iterations.
			return GetPercentageFromPoint( point, 0f, 1f, 0.0025f );
		}

		private float GetPercentageFromPoint( Vector3 point, float pathPercentageFrom, float pathPercentageTo, float accuracy )
		{
			var anchor = pathPercentageFrom.LerpTo( pathPercentageTo, 0.5f );
			var leftSide = pathPercentageFrom.LerpTo( anchor, 0.5f );
			var rightSide = anchor.LerpTo( pathPercentageTo, 0.5f );

			// If the difference is less than our accuracy, just return the anchor.
			if ( MathF.Abs( rightSide - leftSide ) <= accuracy )
				return anchor;

			// If its greater, than work out which side is closer, and do it again with those.
			var leftSideDistance = GetPositionOnCurve( leftSide ).Distance( point );
			var rightSideDistance = GetPositionOnCurve( rightSide ).Distance( point );

			return leftSideDistance < rightSideDistance
				? GetPercentageFromPoint( point, pathPercentageFrom, anchor, accuracy )
				: GetPercentageFromPoint( point, anchor, pathPercentageTo, accuracy );
		}
	}
}
