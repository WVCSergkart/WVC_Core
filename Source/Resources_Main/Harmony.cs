using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace WVC_UltraExpansion
{

	public class WVC_UltraExpansion_Main : Mod
	{
		public WVC_UltraExpansion_Main(ModContentPack content)
			: base(content)
		{
			new Harmony("wvc.sergkart.ultraexpansion").PatchAll();
		}
	}

}
