using System;
using System.Linq;
using Hammer;
using Sandbox;
using SBoxDeathrun.Entities.Attributes;
using SBoxDeathrun.Utils.Helpers;

namespace SBoxDeathrun.Entities.Points
{
	[Library( "deathrun_sbox_initial_spectator_point" )]
	[EditorModel( "models/editor/camera.vmdl" )]
	[EntityTool( "Deathrun Initial Spectator Point", "Deathrun Sbox", "Defines a point what a spectator will see on first join" )]
	[ShowFrustum( "Fov", "ZNear", "ZFar", "Color" )]
	public class InitialSpectatorPoint : Entity
	{
		[Property] public int Fov { get; private set; } = 80;
		[Property] public int ZNear { get; private set; } = 10;
		[Property] public int ZFar { get; private set; } = 80000;
		[Property] private Color Color { get; set; } = Color.Green;

		public static InitialSpectatorPoint Random()
		{
			Host.AssertServer();

			var isp = IEnumerableHelper.Random( All.OfType<InitialSpectatorPoint>() );
			if ( isp.IsValid() == false )
				throw new Exception( $"No DeathrunInitialSpectatorPoint found" );

			return isp;
		}
	}
}
