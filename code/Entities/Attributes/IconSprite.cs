using System;
using System.Text;
using Hammer;

namespace SBoxDeathrun.Entities.Attributes
{
	[AttributeUsage( AttributeTargets.Class )]
	public class IconSprite : MetaDataAttribute
	{
		private string Material { get; set; }

		public IconSprite( string material )
		{
			Material = material;
		}

		public override void AddHeader( StringBuilder sb )
		{
			sb.Append( $"iconsprite( \"{Material}\" )" );
		}
	}
}
