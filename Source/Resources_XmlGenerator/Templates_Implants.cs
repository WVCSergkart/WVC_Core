using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using WVC_UltraExpansion.ImplantGenerator;


namespace WVC_UltraExpansion
{

	public class ImplantGeneratorDef : Def
	{

		[MustTranslate]
		public string labelNoun;

		public List<BodyPartDef> bodyPartDefs;

		public List<ChangesByPart> changesByPart;

	}

	public class ChangesByPart
	{

		public BodyPartDef def;

		[MustTranslate]
		public string label;

		// [MustTranslate]
		// public string labelNoun;

		public List<HediffStage> stages = new();

		public List<HediffCompProperties> comps = new();

		public GraphicData eyeGraphicSouth;

		public GraphicData eyeGraphicEast;

		public float eyeGraphicScale = 1f;

		public List<ThingDefCountClass> costList;

	}

}
