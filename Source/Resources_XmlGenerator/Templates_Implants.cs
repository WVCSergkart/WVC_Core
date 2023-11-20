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

		public TechLevel techLevel;

		public List<BodyPartDef> bodyPartDefs;

		public ThingTemplateDef thingTemplateDef;

		public HediffTemplateDef hediffTemplateDef;

		public CraftTemplateDef craftTemplateDef;

		public SurgeryTemplateDef surgeryTemplateDef;

		public List<SkillRequirement> medicalSkillRequirements;

		public List<SkillRequirement> craftingSkillRequirements;

		public List<ResearchProjectDef> researchPrerequisites;

		public List<MemeDef> memePrerequisitesAny;

		public List<ThingDef> recipeUsers;

		[NoTranslate]
		public List<string> tradeTags;

		[NoTranslate]
		public List<string> techHediffsTags;

		[NoTranslate]
		public List<string> thingSetMakerTags;

		public Color defaultLabelColor = Color.white;

		public AddedBodyPartProps addedPartProps;

		public List<ChangesByPart> changesByPart;

		// public List<StageByPart> stageByPart;

		public List<ThingCategoryDef> thingCategories;

		public GraphicData graphicData;

		public float priceOffset;

	}

	// public class CostByPart
	// {

		// public BodyPartDef def;

		// [MustTranslate]
		// public string label;

		// public List<ThingDefCountClass> costList;

	// }

	// public class StageByPart
	// {

		// public BodyPartDef def;

		// [MustTranslate]
		// public string label;

		// [MustTranslate]
		// public string labelNoun;

		// public List<HediffStage> stages = new();

		// public List<HediffCompProperties> comps = new();

		// public GraphicData eyeGraphicSouth;

		// public GraphicData eyeGraphicEast;

		// public float eyeGraphicScale = 1f;

	// }

	namespace ImplantGenerator
	{

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

		public class ThingTemplateDef : Def
		{

			public Type thingClass = typeof(ThingWithComps);

			public int pathCost;

			public List<StatModifier> statBases;

			public List<ThingDefCountClass> costList;

			public int stackLimit = 3;

			public DrawerType drawerType = DrawerType.MapMeshOnly;

			public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

			public TickerType tickerType;

			public List<CompProperties> comps = new();

			// public List<BodyPartDef> bodyParts = new();

			public ThingCategory category;

		}

		public class CraftTemplateDef : Def
		{

			[MustTranslate]
			public string jobString = "Doing an unknown recipe.";

			// public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

			public WorkTypeDef requiredGiverWorkType;

			public bool dontShowIfAnyIngredientMissing;

			public float workAmount = 15000f;

			public int displayPriority = 150;

			// public bool targetsBodyPart = true;

			// public bool anesthetize = true;

			public StatDef workSpeedStat;

			public StatDef efficiencyStat;

			public ThingFilter fixedIngredientFilter = new();

			public ThingFilter defaultIngredientFilter;

			public Type workerClass = typeof(RecipeWorker);

			[Unsaved(false)]
			public int adjustedCount = 1;

			public SkillDef workSkill;

			public float workSkillLearnFactor = 1f;

			public int targetCountAdjustment = 1;

			public EffecterDef effectWorking;

			public SoundDef soundWorking;

			public bool mechanitorOnlyRecipe;

			public ThingDef unfinishedThingDef;

		}

		public class HediffTemplateDef : Def
		{

			public Type hediffClass = typeof(Hediff_AddedPart);

			public List<HediffCompProperties> comps;

			[MustTranslate]
			public string descriptionShort;

			public List<HediffStage> stages;

			// public ThingDef spawnThingOnRemoved;

			public bool priceImpact = true;

			public List<string> tags;

		}

		public class SurgeryTemplateDef : Def
		{

			[MustTranslate]
			public string jobString = "Doing an unknown recipe.";

			public List<ThingDef> recipeUsers;

			public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

			public DevelopmentalStage? developmentalStageFilter;

			public bool dontShowIfAnyIngredientMissing;

			public float workAmount = 2500f;

			public bool targetsBodyPart = true;

			public bool anesthetize = true;

			public Type workerClass = typeof(Recipe_InstallArtificialBodyPart);

			public List<IngredientCount> ingredients;

			public ThingFilter fixedIngredientFilter;

			// public ThingFilter defaultIngredientFilter;

			[Unsaved(false)]
			public int adjustedCount = 1;

			public HediffDef addsHediff;

			public override IEnumerable<string> ConfigErrors()
			{
				foreach (string item in base.ConfigErrors())
				{
					yield return item;
				}
			}

		}

	}

}
