using System;
using System.Linq;
using Hammer;
using Sandbox;

namespace SBoxDeathrun.Entities.Triggers
{
	[Library( "deathrun_trap" )]
	[EntityTool( "Deathrun Trap", "Deathrun Sbox", "A entity to store information about a trap" )]
	public partial class Trap : Entity
	{
		[Property] [Net] public int Number { get; set; } = 0;
		[Property] [Net] public TrapCamera TrapCamera { get; set; }

		public void Setup()
		{
			TrapCamera = All.OfType<TrapCamera>().FirstOrDefault( tc => tc.EntityName == $"{EntityName}_camera" );
			if ( TrapCamera.IsValid() == false )
				throw new Exception( $"Trap {Number} does not have a camera attached" );
			
			TrapCamera.CalculateDeathPathLocation();
		}
	}
}
