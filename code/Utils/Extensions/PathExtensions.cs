using Sandbox;
using static Sandbox.BasePathEntity;

namespace SBoxDeathrun.Utils.Extensions
{
	public static class PathExtensions
	{
		public static (Vector3 point, Vector3 normal) GetPointAndNormalBetweenNodes( this BasePathEntity path, PathNode start, PathNode end, float t,
			bool reverse = false )
		{
			var trans = path.Transform;
			var pos = trans.PointToWorld( start.Position );
			var tanOut = trans.PointToWorld( start.Position + (reverse ? start.TangentIn : start.TangentOut) );

			var posNext = trans.PointToWorld( end.Position );
			var tanInNext = trans.PointToWorld( end.Position + (reverse ? end.TangentOut : end.TangentIn) );

			var lerp1 = pos.LerpTo( tanOut, t );
			var lerp2 = tanOut.LerpTo( tanInNext, t );
			var lerp3 = tanInNext.LerpTo( posNext, t );
			var lerpAlmost1 = lerp1.LerpTo( lerp2, t );
			var lerpAlmost2 = lerp2.LerpTo( lerp3, t );

			return (lerpAlmost1.LerpTo( lerpAlmost2, t ), (lerpAlmost2 - lerpAlmost1).Normal);
		}
	}
}
