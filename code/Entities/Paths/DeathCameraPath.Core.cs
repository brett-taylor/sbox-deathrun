using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace SBoxDeathrun.Entities.Paths
{
	[Library( "deathrun_sbox_death_camera_path" )]
	public partial class DeathCameraPath : BasePathEntity
	{
		[ServerVar( "dr_death_camera_path_segments" )]
		public static int DEATH_CAMERA_PATH_SEGMENTS { get; set; } = 10;

		public float TotalLength { get; private set; }
		public IReadOnlyList<DeathCameraPathSegment> Segments { get; private set; }

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
		}

		private DeathCameraPathSegment GetSegmentFromPercentage( float currentPathPercentage )
		{
			AssertValidCurrentPathPercentage( currentPathPercentage );
			return Segments.First( s => s.IsPercentageWithinRange( currentPathPercentage ) );
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

		public Rotation GetRotationOnCurve( float currentPathPercentage )
		{
			Vector3 pointOne;
			Vector3 pointTwo;

			if ( currentPathPercentage >= 0.97f )
			{
				pointOne = GetPositionOnCurve( (currentPathPercentage - 0.03f).Clamp( 0f, 1f ) );
				pointTwo = GetPositionOnCurve( currentPathPercentage );
			}
			else
			{
				pointOne = GetPositionOnCurve( currentPathPercentage );
				pointTwo = GetPositionOnCurve( (currentPathPercentage + 0.03f).Clamp( 0f, 1f ) );
			}

			return Rotation.LookAt( (pointTwo - pointOne).Normal );
		}

		public static DeathCameraPath Get()
		{
			return All.OfType<DeathCameraPath>().First();
		}
	}
}
