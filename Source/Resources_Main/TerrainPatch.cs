using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_UltraExpansion
{

	[StaticConstructorOnStartup]
	public static class PostInitializationTerrainAffordanceTweak
	{
		static PostInitializationTerrainAffordanceTweak()
		{
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading)
			{
				if (terrainDef != null)
				{
					terrainDef.affordances.Add(WVC_UltraDefOf.WVC_Ultra_AllAffordances);
				}
			}
		}
	}

}
