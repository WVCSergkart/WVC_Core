using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_UltraExpansion
{

	[StaticConstructorOnStartup]
	public static class Ultra_PostInitialization
	{
		static Ultra_PostInitialization()
		{
			List<string> filter = UltraFilterUtility.BlackListedTerrainDefs();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading)
			{
				if (!filter.Contains(terrainDef.defName))
				{
					terrainDef?.affordances?.Add(WVC_UltraDefOf.WVC_Ultra_AllAffordances);
				}
			}
			if (WVC_Ultra.settings.onlyTechsMode)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.IsUltraDef())
					{
						if (thingDef.thingSetMakerTags != null)
						{
							// Log.Error(thingDef.LabelCap + " patched.");
							thingDef.thingSetMakerTags = null;
							thingDef.tradeTags = new() { "ExoticMisc" };
							thingDef.tradeability = Tradeability.Sellable;
						}
						if (thingDef.techHediffsTags != null)
						{
							thingDef.techHediffsTags = null;
						}
					}
				}
			}
		}
	}

}
