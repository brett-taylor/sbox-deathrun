namespace SBoxDeathrun.Entities.Paths
{
	public partial class DeathCameraPath
	{
		public class DeathCameraPathSegment
		{
			public PathNode From { get; }
			public PathNode To { get; }
			public float Length { get; }
			public float Percentage { get; }
			public float PercentageFrom => PreviousPercentage;
			public float PercentageTo => PreviousPercentage + Percentage;

			private float PreviousPercentage { get; }
			
			public DeathCameraPathSegment( PathNode from, PathNode to, float length, float totalLength, float previousPercentage )
			{
				From = from;
				To = to;
				Length = length;
				Percentage = length / totalLength;
				PreviousPercentage = previousPercentage;
			}

			public bool IsPercentageWithinRange( float percentage )
			{
				// TODO: nice edge case, probably better solution.
				// Cant swap inclusive as it just moves the issue to 0f
				// Limit 0f to 0.99f?
				// Keep this little edge case detection?
				if ( PercentageTo >= 1f && percentage >= 1f )
					return true;
				
				return percentage >= PercentageFrom && percentage < PercentageTo;
			}
		}
	}
}
