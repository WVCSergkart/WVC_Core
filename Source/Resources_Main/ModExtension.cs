using RimWorld;
using System.Collections.Generic;
using Verse;

// namespace WVC
namespace WVC_UltraExpansion
{

	public class UltraExpansionListDef : Def
	{
		public List<string> blackListedTerrainDefs = new();
		public List<string> mechDefNameShouldNotContain = new();
	}

}
