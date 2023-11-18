using RimWorld;
using System.Collections.Generic;
using Verse;


namespace WVC_UltraExpansion
{

	public static class UltraFilterUtility
	{

		public static List<string> BlackListedTerrainDefs()
		{
			List<string> list = new();
			foreach (UltraExpansionListDef item in DefDatabase<UltraExpansionListDef>.AllDefsListForReading)
			{
				list.AddRange(item.blackListedTerrainDefs);
			}
			return list;
		}

	}
}
