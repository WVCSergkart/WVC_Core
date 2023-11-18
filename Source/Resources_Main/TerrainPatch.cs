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
			List<string> filter = UltraFilterUtility.BlackListedTerrainDefs();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading)
			{
				if (!filter.Contains(terrainDef.defName))
				{
					terrainDef?.affordances?.Add(WVC_UltraDefOf.WVC_Ultra_AllAffordances);
				}
			}
		}
	}

}
