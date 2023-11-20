using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
// using System.Collections.IEnumerable;
using System.Linq;
using Verse;
// using WVC_UltraExpansion.ImplantGenerator;
// using static RimWorld.BaseGen.SymbolStack;


namespace WVC_UltraExpansion
{

	namespace ImplantGenerator
	{

		[HarmonyPatch(typeof(ThingDefGenerator_Neurotrainer), "ImpliedThingDefs")]
		public static class WVC_ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch
		{
			[HarmonyPostfix]
			public static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values)
			{
				List<ThingDef> thingDefList = values.ToList();
				// if (WVC_Biotech.settings.serumsForAllXenotypes)
				// {
					// foreach (SerumTemplateDef serumTemplate in DefDatabase<SerumTemplateDef>.AllDefsListForReading)
					// {
						// if (serumTemplate.serumTagName.Contains("BaseSerumsGenerator") && WVC_Biotech.settings.serumsForAllXenotypes_GenBase)
						// {
							// foreach (XenotypeDef allDef in XenotypeFilterUtility.WhiteListedXenotypes(true))
							// {
								// thingDefList.Add(TemplatesUtility.GetFromThingTemplate(serumTemplate, allDef, allDef.index * 1000));
							// }
						// }
					// }
				// }
				foreach (ImplantGeneratorDef generatorDef in DefDatabase<ImplantGeneratorDef>.AllDefsListForReading)
				{
					foreach (BodyPartDef bodyPartDef in generatorDef.bodyPartDefs)
					{
						thingDefList.Add(TemplatesUtility.GetFromTemplate_ThingDef(generatorDef.thingTemplateDef, bodyPartDef, generatorDef));
					}
				}
				return thingDefList;
			}
		}

	}

}
