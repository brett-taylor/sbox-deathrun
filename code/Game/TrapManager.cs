using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using SBoxDeathrun.Entities.Triggers;

namespace SBoxDeathrun
{
	public partial class TrapManager : Entity
	{
		[Net] public List<Trap> Traps { get; private set; }

		public void PostLevelLoaded()
		{
			Host.AssertServer();

			Traps = All.OfType<Trap>().ToList();
			SanityCheckTraps();

			foreach ( var trap in Traps )
				trap.Setup();
		}

		private void SanityCheckTraps()
		{
			if ( !Traps.Any() )
				Log.Error( $"No traps found, probably an issue?" );

			var trapNumbers = Traps.Select( t => t.Number ).OrderBy( i => i ).ToList();
			Log.Info( $"Traps Found: {string.Join( ", ", trapNumbers )}" );

			// Check the list of trapNumbers is 0...length, no duplicates and in order.
			var goodTrapNumbers = Enumerable.Range( 1, Traps.Count ).All( i => trapNumbers[i - 1] == i );
			if ( !goodTrapNumbers )
				throw new Exception( $"Trap numbers are not 1 to {trapNumbers.Count}. Trap numbers found: {string.Join( ", ", trapNumbers )}" );
		}
	}
}
