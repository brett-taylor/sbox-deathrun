using System;
using System.Collections.Generic;
using System.Linq;
using Hammer;
using Sandbox;
using SBoxDeathrun.Team;
using SBoxDeathrun.Utils.Helpers;

namespace SBoxDeathrun.Entities
{
	[Library( "deathrun_sbox_spawn_point" )]
	[EditorModel( "models/editor/playerstart.vmdl" )]
	[EntityTool( "Deathrun Player Spawnpoint", "Deathrun Sbox", "Defines a point where the player can (re)spawn" )]
	public class DeathrunSpawnPoint : Entity
	{
		private static readonly IReadOnlyList<TeamType> VALID_TEAM_TYPES = ListHelpers.Of( TeamType.DEATH, TeamType.RUNNER );

		[Property( Title = "The team this spawn point is for" )]
		public TeamType Team { get; private set; } = TeamType.SPECTATOR;

		public override void Spawn()
		{
			if ( VALID_TEAM_TYPES.Contains( Team ) == false )
				throw new Exception( $"DeathrunSpawnPoint has Team Spectator Spawn. Did you forget to set it?" );
		}

		public static DeathrunSpawnPoint Random()
		{
			Host.AssertServer();

			var sp = IEnumerableHelpers.Random( All.OfType<DeathrunSpawnPoint>() );
			if ( sp.IsValid() == false )
				throw new Exception( $"No DeathrunSpawnPoint found" );

			return sp;
		}

		public static DeathrunSpawnPoint SpawnPointForTeam( TeamType team )
		{
			Host.AssertServer();

			var sp = IEnumerableHelpers.Random( All
				.OfType<DeathrunSpawnPoint>()
				.Where( dsp => dsp.Team == team )
			);

			if ( sp.IsValid() == false )
				throw new Exception( $"No DeathrunSpawnPoint for {team.NiceName()} found." );

			return sp;
		}
	}
}
