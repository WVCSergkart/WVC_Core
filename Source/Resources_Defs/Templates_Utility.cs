using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;


namespace WVC_UltraExpansion
{

	namespace ImplantGenerator
	{

		public static class TemplatesUtility
		{

			public static ThingDef GetFromTemplate_ThingDef(ThingTemplateDef template, BodyPartDef def, ImplantGeneratorDef implantGeneratorDef)
			{

				ThingDef thingDef = new()
				{
					defName = implantGeneratorDef.defName + "_" + def.defName + "_Ultra",
					label = implantGeneratorDef.label.Formatted(def.label),
					description = implantGeneratorDef.description.Formatted(def.label),
					descriptionHyperlinks = new(),
					techLevel = implantGeneratorDef.techLevel,
					thingCategories = implantGeneratorDef.thingCategories,
					graphicData = implantGeneratorDef.graphicData,
					costList = template.costList,
					stackLimit = template.stackLimit,
					statBases = template.statBases,
					tradeTags = implantGeneratorDef.tradeTags,
					techHediffsTags = implantGeneratorDef.techHediffsTags,
					thingSetMakerTags = implantGeneratorDef.thingSetMakerTags,
					thingClass = template.thingClass,
					category = template.category,
					drawerType = template.drawerType,
					altitudeLayer = template.altitudeLayer,
					tickerType = template.tickerType,
					// label = template.label,
					// label = template.label,
					// label = template.label,
					modExtensions = template.modExtensions,
					comps = template.comps,
					pathCost = template.pathCost,
					allowedArchonexusCount = 1,
					isTechHediff = true,
					alwaysHaulable = true,
					selectable = true,
					useHitPoints = true,
					drawGUIOverlay = true
				};
				List<ChangesByPart> costByPart = implantGeneratorDef.changesByPart;
				for (int i = 0; i < costByPart.Count; i++)
				{
					if (def == costByPart[i].def)
					{
						if (costByPart[i].label != null)
						{
							thingDef.label = implantGeneratorDef.label.Formatted(costByPart[i].label);
						}
						thingDef.costList = costByPart[i].costList;
						break;
					}
				}
				if (template.hediffTemplateDef != null)
				{
					DefGenerator.AddImpliedDef(GetFromTemplate_HediffDef(template.hediffTemplateDef, def, thingDef, implantGeneratorDef));
				}
				if (template.craftTemplateDef != null)
				{
					DefGenerator.AddImpliedDef(GetFromTemplate_CraftDef(template.craftTemplateDef, thingDef, implantGeneratorDef));
				}
				return thingDef;
			}

			public static RecipeDef GetFromTemplate_CraftDef(CraftTemplateDef template, ThingDef thingDef, ImplantGeneratorDef implantGeneratorDef)
			{
				string text = thingDef.label;
				RecipeDef recipeDef = new()
				{
					defName = "Make_" + thingDef.defName,
					label = "RecipeMake".Translate(text),
					jobString = "RecipeMakeJobString".Translate(text),
					modContentPack = thingDef.modContentPack,
					displayPriority = template.displayPriority,
					workAmount = template.workAmount * template.adjustedCount,
					workSpeedStat = template.workSpeedStat,
					efficiencyStat = template.efficiencyStat,
					useIngredientsForColor = false,
					defaultIngredientFilter = template.defaultIngredientFilter,
					targetCountAdjustment = template.targetCountAdjustment,
					skillRequirements = implantGeneratorDef.craftingSkillRequirements,
					workSkill = template.workSkill,
					workSkillLearnFactor = template.workSkillLearnFactor,
					requiredGiverWorkType = template.requiredGiverWorkType,
					unfinishedThingDef = template.unfinishedThingDef,
					recipeUsers = implantGeneratorDef.recipeUsers.ListFullCopyOrNull(),
					mechanitorOnlyRecipe = template.mechanitorOnlyRecipe,
					effectWorking = template.effectWorking,
					soundWorking = template.soundWorking,
					memePrerequisitesAny = implantGeneratorDef.memePrerequisitesAny,
					researchPrerequisites = implantGeneratorDef.researchPrerequisites
					// researchPrerequisites = new List<ResearchProjectDef> { },
					// factionPrerequisiteTags = template.factionPrerequisiteTags,
					// fromIdeoBuildingPreceptOnly = template.fromIdeoBuildingPreceptOnly
				};
				recipeDef.descriptionHyperlinks = recipeDef.products.Select((ThingDefCountClass p) => new DefHyperlink(p.thingDef)).ToList();
				recipeDef.products.Add(new ThingDefCountClass(thingDef, 1 * template.adjustedCount));
				string[] items = recipeDef.products.Select((ThingDefCountClass p) => (p.count != 1) ? p.Label : Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label)).ToArray();
				recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(useAnd: true));
				if (template.adjustedCount != 1)
				{
					text = text + " x" + template.adjustedCount;
				}
				RecipeDefGenerator.SetIngredients(recipeDef, thingDef, template.adjustedCount);
				if (thingDef.costListForDifficulty != null)
				{
					recipeDef.regenerateOnDifficultyChange = true;
				}
				return recipeDef;
			}

			public static HediffDef GetFromTemplate_HediffDef(HediffTemplateDef template, BodyPartDef def, ThingDef thingDef, ImplantGeneratorDef implantGeneratorDef)
			{

				HediffDef hediffDef = new()
				{
					// defName = implantGeneratorDef.defName + "_" + def.defName,
					defName = "Hediff_" + thingDef.defName,
					hediffClass = template.hediffClass,
					label = implantGeneratorDef.label.Formatted(def.label),
					labelNoun = implantGeneratorDef.labelNoun.Formatted(def.label),
					descriptionHyperlinks = new(),
					priceImpact = template.priceImpact,
					priceOffset = implantGeneratorDef.priceOffset,
					description = implantGeneratorDef.description.Formatted(def.label),
					descriptionShort = template.descriptionShort.Formatted(def.label),
					defaultLabelColor = implantGeneratorDef.defaultLabelColor,
					tags = template.tags,
					addedPartProps = implantGeneratorDef.addedPartProps,
					// eyeGraphicScale = template.eyeGraphicScale,
					// eyeGraphicEast = template.eyeGraphicEast,
					// eyeGraphicSouth = template.eyeGraphicSouth,
					// comps = template.comps,
					// stages = template.stages,
					modExtensions = template.modExtensions,
					spawnThingOnRemoved = thingDef,
					// spawnThingOnRemoved = template.spawnThingOnRemoved,
					allowMothballIfLowPriorityWorldPawn = true,
					countsAsAddedPartOrImplant = true,
					isBad = false,
					scenarioCanAdd = false
				};
				List<ChangesByPart> stageByPart = implantGeneratorDef.changesByPart;
				for (int i = 0; i < stageByPart.Count; i++)
				{
					if (def == stageByPart[i].def)
					{
						if (stageByPart[i].label != null)
						{
							hediffDef.label = implantGeneratorDef.label.Formatted(stageByPart[i].label);
							hediffDef.labelNoun = implantGeneratorDef.labelNoun.Formatted(stageByPart[i].label);
						}
						hediffDef.stages = stageByPart[i].stages;
						hediffDef.comps = stageByPart[i].comps;
						break;
					}
				}
				if (template.stages != null)
				{
					foreach (HediffStage item in template.stages)
					{
						hediffDef.stages.Add(item);
					}
				}
				if (template.comps != null)
				{
					foreach (HediffCompProperties item in template.comps)
					{
						hediffDef.comps.Add(item);
					}
				}
				if (template.surgeryTemplateDef != null)
				{
					DefGenerator.AddImpliedDef(GetFromTemplate_SurgeryDef(template.surgeryTemplateDef, def, thingDef, hediffDef, implantGeneratorDef));
				}
				hediffDef.descriptionHyperlinks.Add(thingDef);
				return hediffDef;
			}

			public static RecipeDef GetFromTemplate_SurgeryDef(SurgeryTemplateDef template, BodyPartDef def, ThingDef thingDef, HediffDef hediffDef, ImplantGeneratorDef implantGeneratorDef)
			{
				RecipeDef recipeDef = new()
				{
					defName = "Install_" + hediffDef.defName,
					label = template.label + " " + thingDef.label,
					description = template.description + " " + thingDef.label + ".",
					descriptionHyperlinks = new(),
					jobString = template.jobString + " " + thingDef.label + ".",
					recipeUsers = template.recipeUsers,
					surgeryOutcomeEffect = template.surgeryOutcomeEffect,
					dontShowIfAnyIngredientMissing = template.dontShowIfAnyIngredientMissing,
					workAmount = template.workAmount,
					anesthetize = template.anesthetize,
					targetsBodyPart = template.targetsBodyPart,
					workerClass = template.workerClass,
					adjustedCount = template.adjustedCount,
					// ingredients = template.ingredients,
					// fixedIngredientFilter = template.fixedIngredientFilter,
					skillRequirements = implantGeneratorDef.medicalSkillRequirements,
					appliedOnFixedBodyParts = new List<BodyPartDef>
					{
						def
					},
					addsHediff = hediffDef,
					modExtensions = template.modExtensions
				};
				// recipeDef.ingredients = template.ingredients;
				// recipeDef.fixedIngredientFilter = template.fixedIngredientFilter;
				IngredientCount ingredientCount = new();
				ingredientCount.SetBaseCount(1);
				ingredientCount.filter.SetAllow(thingDef, true);
				recipeDef.ingredients.Add(ingredientCount);
				recipeDef.fixedIngredientFilter.SetAllow(thingDef, true);
				if (template.ingredients != null)
				{
					foreach (IngredientCount item in template.ingredients)
					{
						recipeDef.ingredients.Add(item);
					}
				}
				if (template.fixedIngredientFilter != null)
				{
					recipeDef.fixedIngredientFilter.SetAllowAll(template.fixedIngredientFilter, true);
				}
				// recipeDef.appliedOnFixedBodyParts.Add(def);
				recipeDef.descriptionHyperlinks.Add(thingDef);
				recipeDef.descriptionHyperlinks.Add(hediffDef);
				thingDef.descriptionHyperlinks.Add(recipeDef);
				return recipeDef;
			}

		}
	}

}
