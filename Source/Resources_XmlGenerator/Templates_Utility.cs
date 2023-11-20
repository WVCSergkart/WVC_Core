using RimWorld;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Verse;


namespace WVC_UltraExpansion
{

	namespace ImplantGenerator
	{

		public static class TemplatesUtility
		{

			public static void GetOrCreateStorageModFolders(out string path, out bool canGenerate)
			{
				canGenerate = false;
				DirectoryInfo directoryInfo = new(Path.Combine(new DirectoryInfo(GenFilePaths.ModsFolderPath).ToString(), "WVC_Core"));
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				DirectoryInfo directoryInfo4 = new(Path.Combine(directoryInfo.ToString(), "1.4"));
				if (!directoryInfo4.Exists)
				{
					directoryInfo4.Create();
				}
				DirectoryInfo directoryInfo5 = new(Path.Combine(directoryInfo4.ToString(), "Defs"));
				if (!directoryInfo5.Exists)
				{
					directoryInfo5.Create();
				}
				DirectoryInfo directoryInfo6 = new(Path.Combine(directoryInfo5.ToString(), "Implants"));
				if (!directoryInfo6.Exists)
				{
					canGenerate = true;
					directoryInfo6.Create();
				}
				path = Path.Combine(directoryInfo6.ToString());
			}

			public static XElement GenerateThingDefFile(ThingDef def, ImplantGeneratorDef generatorDef)
			{
				// XElement xElement = new("ThingDef" + " ParentNameTag", new XElement("defName", def.defName), new XElement("label", def.label), new XElement("description", def.description), new XElement("descriptionHyperlinks", def.descriptionHyperlinks), new XElement("techLevel", def.techLevel), new XElement("thingCategories", def.thingCategories), new XElement("graphicData", def.graphicData), new XElement("costList", def.costList), new XElement("stackLimit", def.stackLimit), new XElement("statBases", def.statBases), new XElement("tradeTags"), new XElement("techHediffsTags"), new XElement("thingSetMakerTags"), new XElement("thingClass", def.thingClass), new XElement("category", def.category), new XElement("drawerType", def.drawerType), new XElement("altitudeLayer", def.altitudeLayer), new XElement("tickerType", def.tickerType), new XElement("modExtensions", def.modExtensions), new XElement("comps", def.comps), new XElement("pathCost", def.pathCost), new XElement("allowedArchonexusCount", def.allowedArchonexusCount), new XElement("isTechHediff", def.isTechHediff), new XElement("alwaysHaulable", def.alwaysHaulable), new XElement("selectable", def.selectable), new XElement("useHitPoints", def.useHitPoints), new XElement("drawGUIOverlay", def.drawGUIOverlay));
				XElement xElement = new("ThingDef", new XElement("defName", def.defName), new XElement("label", def.label), new XElement("description", def.description), new XElement("descriptionHyperlinks"), new XElement("costList"));
				// XElement tradeTags = xElement.Element("tradeTags");
				// foreach (var i in def.tradeTags)
				// {
					// tradeTags.Add(new XElement("li", i));
				// }
				// XElement techHediffsTags = xElement.Element("techHediffsTags");
				// foreach (var i in def.techHediffsTags)
				// {
					// techHediffsTags.Add(new XElement("li", i));
				// }
				// XElement thingSetMakerTags = xElement.Element("thingSetMakerTags");
				// foreach (var i in def.thingSetMakerTags)
				// {
					// thingSetMakerTags.Add(new XElement("li", i));
				// }
				XElement costList = xElement.Element("costList");
				foreach (var i in def.costList)
				{
					costList.Add(new XElement(i.thingDef.defName, i.count));
				}
				XElement descriptionHyperlinks = xElement.Element("descriptionHyperlinks");
				foreach (var i in def.descriptionHyperlinks)
				{
					if (i.def is ThingDef)
					{
						descriptionHyperlinks.Add(new XElement("ThingDef", i.def.defName));
					}
					if (i.def is RecipeDef)
					{
						descriptionHyperlinks.Add(new XElement("RecipeDef", i.def.defName));
					}
					if (i.def is HediffDef)
					{
						descriptionHyperlinks.Add(new XElement("HediffDef", i.def.defName));
					}
				}
				// XElement costList = xElement.Element("costList");
				// foreach (var i in def.costList)
				// {
					// costList.Add(new XElement(i.thingDef.defName, i.count));
				// }
				// XmlAttribute xmlAttribute = xElement.OwnerDocument.CreateAttribute("ParentName");
				// xmlAttribute.Value = "WVC_ParentName";
				// xElement.Attributes.Append(xmlAttribute);
				string name = "ParentName";
				xElement.SetAttributeValue(name, "WVC_Ultra_ImplantThingDef_" + generatorDef.defName);
				return xElement;
			}

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
				PostInitializationDefGenerator.thingDefs.Add(thingDef);
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

			public static XElement GenerateHediffDefFile(HediffDef def, ImplantGeneratorDef generatorDef)
			{
				XElement xElement = new("HediffDef", new XElement("defName", def.defName), new XElement("label", def.label), new XElement("description", def.description), new XElement("descriptionHyperlinks"), new XElement("spawnThingOnRemoved", def.spawnThingOnRemoved.defName), new XElement("stages"), new XElement("comps"));
				XElement descriptionHyperlinks = xElement.Element("descriptionHyperlinks");
				foreach (var i in def.descriptionHyperlinks)
				{
					if (i.def is ThingDef)
					{
						descriptionHyperlinks.Add(new XElement("ThingDef", i.def.defName));
					}
					if (i.def is RecipeDef)
					{
						descriptionHyperlinks.Add(new XElement("RecipeDef", i.def.defName));
					}
					if (i.def is HediffDef)
					{
						descriptionHyperlinks.Add(new XElement("HediffDef", i.def.defName));
					}
				}
				// XElement stages = xElement.Element("stages");
				// List<ChangesByPart> stageByPart = implantGeneratorDef.changesByPart;
				// for (int i = 0; i < stageByPart.Count; i++)
				// {
					// if (bodyPartDef == stageByPart[i].def)
					// {
						// stages.Add(stageByPart[i].stages);
						// break;
					// }
				// }
				// XElement comps = xElement.Element("comps");
				// for (int i = 0; i < stageByPart.Count; i++)
				// {
					// if (bodyPartDef == stageByPart[i].def)
					// {
						// comps.Add(stageByPart[i].comps);
						// break;
					// }
				// }
				XElement stages = xElement.Element("stages");
				foreach (var stage in def.stages)
				{
					stages.Add(new XElement("li"));
					XElement li = stages.Element("li");
					if (!stage.statOffsets.NullOrEmpty())
					{
						li.Add(new XElement("statOffsets"));
						XElement statOffsets = li.Element("statOffsets");
						foreach (var statOffset in stage.statOffsets)
						{
							string mayRequire = "MayRequire";
							string statName = statOffset.stat.defName;
							statOffsets.Add(new XElement(statName, statOffset.value));
							if (statOffset.stat == StatDefOf.MechBandwidth || statOffset.stat == StatDefOf.ToxicEnvironmentResistance)
							{
								statOffsets.Element(statName).SetAttributeValue(mayRequire, "Ludeon.RimWorld.Biotech");
							}
						}
					}
					if (stage.hungerRateFactor != 1f)
					{
						li.Add(new XElement("hungerRateFactor", stage.hungerRateFactor));
					}
					if (stage.foodPoisoningChanceFactor != 1f)
					{
						li.Add(new XElement("foodPoisoningChanceFactor", stage.foodPoisoningChanceFactor));
					}
					if (stage.naturalHealingFactor != -1f)
					{
						li.Add(new XElement("naturalHealingFactor", stage.naturalHealingFactor));
					}
					if (stage.totalBleedFactor != 1f)
					{
						li.Add(new XElement("totalBleedFactor", stage.totalBleedFactor));
					}
				}
				if (!def.comps.NullOrEmpty())
				{
					XElement comps = xElement.Element("comps");
					foreach (var comp in def.comps)
					{
						comps.Add(new XElement("li"));
						XElement li = comps.Element("li");
						string nameClass = "Class";
						li.SetAttributeValue(nameClass, "HediffCompProperties_VerbGiver");
						li.Add(new XElement("tools"));
						XElement tools = li.Element("tools");
						// tools = comp as HediffCompProperties_VerbGiver;
						HediffCompProperties_VerbGiver verb = (HediffCompProperties_VerbGiver)comp;
						foreach (var tool in verb.tools)
						{
							tools.Add(new XElement("li"));
							XElement tools_li = tools.Element("li");
							tools_li.Add(new XElement("label", tool.label));
							tools_li.Add(new XElement("capacities"));
							XElement capacities = tools_li.Element("capacities");
							foreach (var capacitie in tool.capacities)
							{
								capacities.Add(new XElement("li", capacitie));
							}
							tools_li.Add(new XElement("power", tool.power));
							tools_li.Add(new XElement("cooldownTime", tool.cooldownTime));
							tools_li.Add(new XElement("soundMeleeHit", tool.soundMeleeHit));
							tools_li.Add(new XElement("soundMeleeMiss", tool.soundMeleeMiss));
						}
					}
				}
				string name = "ParentName";
				xElement.SetAttributeValue(name, "WVC_Ultra_ImplantHediffDef_" + generatorDef.defName);
				return xElement;
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
				PostInitializationDefGenerator.hediffDefs.Add(hediffDef);
				return hediffDef;
			}

			public static XElement GenerateSurgeryDefFile(RecipeDef def)
			{
				XElement xElement = new("RecipeDef", new XElement("defName", def.defName), new XElement("label", def.label), new XElement("description", def.description), new XElement("descriptionHyperlinks"), new XElement("appliedOnFixedBodyParts"), new XElement("addsHediff", def.addsHediff.defName), new XElement("ingredients"), new XElement("fixedIngredientFilter"));
				XElement descriptionHyperlinks = xElement.Element("descriptionHyperlinks");
				foreach (var i in def.descriptionHyperlinks)
				{
					if (i.def is ThingDef)
					{
						descriptionHyperlinks.Add(new XElement("ThingDef", i.def.defName));
					}
					if (i.def is RecipeDef)
					{
						descriptionHyperlinks.Add(new XElement("RecipeDef", i.def.defName));
					}
					if (i.def is HediffDef)
					{
						descriptionHyperlinks.Add(new XElement("HediffDef", i.def.defName));
					}
				}
				XElement appliedOnFixedBodyParts = xElement.Element("appliedOnFixedBodyParts");
				foreach (var i in def.appliedOnFixedBodyParts)
				{
					appliedOnFixedBodyParts.Add(new XElement("li", i.defName));
				}
				XElement ingredients = xElement.Element("ingredients");
				XElement fixedIngredientFilter = xElement.Element("fixedIngredientFilter");
				foreach (var ingredient in def.ingredients)
				{
					ingredients.Add(new XElement("li"));
					XElement li = ingredients.Element("li");
					li.Add(new XElement("filter"));
					XElement filter = li.Element("filter");
					filter.Add(new XElement("thingDefs"));
					XElement filterDefs = filter.Element("thingDefs");
					filterDefs.Add(new XElement("li", ingredient.FixedIngredient.defName));

					fixedIngredientFilter.Add(new XElement("thingDefs"));
					XElement fixedIngredientFilterli = fixedIngredientFilter.Element("thingDefs");
					fixedIngredientFilterli.Add(new XElement("li", ingredient.FixedIngredient.defName));
				}
				string name = "ParentName";
				xElement.SetAttributeValue(name, "WVC_Ultra_ImplantSurgeryDef_Base");
				return xElement;
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
				IngredientCount ingredientCount = new();
				ingredientCount.SetBaseCount(1);
				ingredientCount.filter.SetAllow(thingDef, true);
				// recipeDef.ingredients = template.ingredients.ListFullCopyOrNull();
				recipeDef.ingredients.Add(ingredientCount);
				// recipeDef.fixedIngredientFilter = template.fixedIngredientFilter;
				recipeDef.fixedIngredientFilter.SetAllow(thingDef, true);
				// recipeDef.appliedOnFixedBodyParts.Add(def);
				recipeDef.descriptionHyperlinks.Add(thingDef);
				recipeDef.descriptionHyperlinks.Add(hediffDef);
				thingDef.descriptionHyperlinks.Add(recipeDef);
				hediffDef.descriptionHyperlinks.Add(recipeDef);
				PostInitializationDefGenerator.recipeDefs.Add(recipeDef);
				return recipeDef;
			}

		}
	}

}
