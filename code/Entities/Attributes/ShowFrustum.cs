using System;
using System.Text;
using Hammer;

namespace SBoxDeathrun.Entities.Attributes
{
	[AttributeUsage( AttributeTargets.Class )]
	public class ShowFrustum : MetaDataAttribute
	{
		private string Fov { get; set; }
		private string Near { get; set; }
		private string Far { get; set; }
		private string Color { get; set; }

		public ShowFrustum( string fov, string near, string far, string color )
		{
			Fov = fov;
			Near = near;
			Far = far;
			Color = color;
		}

		public override void AddHeader( StringBuilder sb )
		{
			sb.Append( $"frustum( {Fov}, {Near}, {Far}, {Color} )" );
		}
	}
}
