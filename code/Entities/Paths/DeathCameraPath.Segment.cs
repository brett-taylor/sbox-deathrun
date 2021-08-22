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

			public bool IsPercentageWithinRange( float percentage, float totalPathPercentage )
			{
				if ( PercentageTo >= totalPathPercentage && percentage >= totalPathPercentage )
					return true;

				return percentage >= PercentageFrom && percentage < PercentageTo;
			}
		}
	}
}
