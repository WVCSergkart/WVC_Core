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

		// public string surgeryTag = "Base";

		public List<BodyPartDef> bodyPartDefs;

		public List<ChangesByPart> changesByPart;

		public string mayRequire;

		// Implant or AddedPart
		public string inNameTag = "AddedPart";

		public bool generateRemoveRecipe = false;

		public bool forAnimals = false;

		public bool forMechs = false;

		public JobDef installJobDef;

		public ThingDef installMoteDef;

		public string inheritSurgery = "WVC_Ultra_AddedPartSurgeryDef_Base";

		public string inheritSurgeryRemove = "WVC_Ultra_ImplantSurgeryRemoveDef_Base";

		public string inheritHediff = "WVC_Ultra_AddedBodyPart_Base";

		public string inheritThing = "WVC_Ultra_ThingDefBodyParts_Base";

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

		public float eyeGraphicScale = 0.2f;

		public float partEfficiency = 1f;

		public string texPath;

		public List<ThingDefCountClass> costList;

	}

}
