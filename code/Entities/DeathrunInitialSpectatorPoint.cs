using System;
using System.Linq;
using Hammer;
using Sandbox;
using SBoxDeathrun.Utils.Helpers;

namespace SBoxDeathrun.Entities
{
	[Library( "deathrun_sbox_initial_spectator_point" )]
	[EditorModel( "models/editor/camera.vmdl" )]
	[EntityTool( "Deathrun Initial Spectator Point", "Deathrun Sbox", "Defines a point what a spectator will see on first join" )]
	public class DeathrunInitialSpectatorPoint : Entity
	{
		public static DeathrunInitialSpectatorPoint Random()
		{
			Host.AssertServer();

			var isp = IEnumerableHelpers.Random( All.OfType<DeathrunInitialSpectatorPoint>() );
			if ( isp.IsValid() == false )
				throw new Exception( $"No DeathrunInitialSpectatorPoint found" );

			return isp;
		}
	}
}
