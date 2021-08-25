using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Entities.Triggers;

namespace SBoxDeathrun
{
	public class TrapManager : NetworkComponent
	{
		private IReadOnlyCollection<Trap> Traps { get; set; }

		public void PostLevelLoaded()
		{
			Host.AssertServer();

			Traps = Entity.All.OfType<Trap>().ToList();
			SanityCheckTraps();

			foreach ( var trap in Traps )
				trap.Setup();
		}

		private void SanityCheckTraps()
		{
			var trapNumbers = Traps.Select( t => t.Number ).OrderBy( i => i ).ToList();

			if ( !Traps.Any() )
				Log.Warning( $"No traps found, probably an issue?" );

			Log.Info( $"Traps Found: {string.Join( ", ", trapNumbers )}" );

			// Check the list of trapNumbers is 0...length, no duplicates and in order.
			var goodTrapNumbers = Enumerable.Range( 1, Traps.Count ).All( i => trapNumbers[i - 1] == i );
			if ( !goodTrapNumbers )
				throw new Exception( $"Trap numbers are not 1 to {trapNumbers.Count}. Trap numbers found: {string.Join( ", ", trapNumbers )}" );
		}
	}
}
