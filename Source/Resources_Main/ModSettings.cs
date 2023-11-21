using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using Verse;

namespace WVC_UltraExpansion
{

	public class WVC_UltraSettings : ModSettings
	{
		// Graphic
		public bool implantGenerator_SaveBaseImplants = false;
		public bool implantGenerator_SaveRecipeDefsInList = false;
		public bool implantGenerator_FullLogging = false;

		public IEnumerable<string> GetEnabledSettings => from specificSetting in GetType().GetFields()
														 where specificSetting.FieldType == typeof(bool) && (bool)specificSetting.GetValue(this)
														 select specificSetting.Name;

		public override void ExposeData()
		{
			Scribe_Values.Look(ref implantGenerator_SaveBaseImplants, "implantGenerator_SaveBaseImplants", defaultValue: false);
			Scribe_Values.Look(ref implantGenerator_SaveRecipeDefsInList, "implantGenerator_SaveRecipeDefsInList", defaultValue: false);
			Scribe_Values.Look(ref implantGenerator_FullLogging, "implantGenerator_FullLogging", defaultValue: false);
		}
	}

	public class WVC_Ultra : Mod
	{
		public static WVC_UltraSettings settings;

		private int PageIndex = 0;

		private static Vector2 scrollPosition = Vector2.zero;

		public WVC_Ultra(ModContentPack content) : base(content)
		{
			settings = GetSettings<WVC_UltraSettings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Rect rect = new(inRect);
			rect.y = inRect.y + 40f;
			Rect baseRect = rect;
			rect = new Rect(inRect)
			{
				height = inRect.height - 40f,
				y = inRect.y + 40f
			};
			Rect rect2 = rect;
			Widgets.DrawMenuSection(rect2);
			List<TabRecord> tabs = new()
			{
				new TabRecord("WVC_UltraSettings_Tab_General".Translate(), delegate
				{
					PageIndex = 0;
					WriteSettings();
				}, PageIndex == 0),
				// new TabRecord("WVC_UltraSettings_Tab_XenotypesFilter".Translate(), delegate
				// {
					// PageIndex = 1;
					// WriteSettings();
				// }, PageIndex == 1)
			};
			TabDrawer.DrawTabs(baseRect, tabs);
			switch (PageIndex)
			{
				case 0:
					GeneralSettings(rect2.ContractedBy(15f));
					break;
				// case 1:
					// XenotypeFilterSettings(rect2.ContractedBy(15f));
					// break;
			}
		}

		// General Settings
		// General Settings
		// General Settings

		private void GeneralSettings(Rect inRect)
		{
			Rect outRect = new(inRect.x, inRect.y, inRect.width, inRect.height);
			// Rect rect = new(0f, 0f, inRect.width, inRect.height);
			Rect rect = new(0f, 0f, inRect.width - 30f, inRect.height * 2);
			// var labelRect = new Rect(rect.x + 5, rect.y, 60, 24);
			// var resetRect = new Rect(labelRect.x, labelRect.yMax + 5, 265, 24f);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect);
			Listing_Standard listingStandard = new();
			listingStandard.Begin(rect);
			// ===============
			// ===============
			if (Prefs.DevMode)
			{
				// Widgets.Label(labelRect, "WVC_UltraSettings_Label_DEV".Translate());
				// if (Widgets.ButtonText(resetRect, "WVC_UltraSettings_Label_Generator".Translate()))
				// {
					// ImplantGenerator.TemplatesUtility.GeneratorInitialization();
				// }
				// listingStandard.Label("WVC_UltraSettings_Label_DEV".Translate() + ":", -1);
				// listingStandard.CheckboxLabeled("WVC_UltraSettings_Label_Generator".Translate(), ref settings.implantGenerator, "WVC_UltraSettings_Tooltip_Generator".Translate());
				listingStandard.Label("WVC_UltraSettings_Label_DEV".Translate() + ":", -1,"WVC_UltraSettings_Tooltip_DEV".Translate());
				listingStandard.CheckboxLabeled("DEV: Save Defs", ref settings.implantGenerator_SaveBaseImplants);
				// listingStandard.CheckboxLabeled("DEV: Generate Base Implants", ref settings.implantGenerator_BaseImplants);
				listingStandard.CheckboxLabeled("DEV: Save RecipeDef defNames in patch", ref settings.implantGenerator_SaveRecipeDefsInList);
				listingStandard.CheckboxLabeled("DEV: Defs Logging", ref settings.implantGenerator_FullLogging);
				if (listingStandard.ButtonText("DEV: Re-Generate"))
				{
					ImplantGenerator.GeneratorMainUtility.GeneratorInitialization();
				}
			}
			listingStandard.End();
			Widgets.EndScrollView();
		}

		public override string SettingsCategory()
		{
			return "WVC - " + "WVC_UltraSettings".Translate();
		}
	}

	public class PatchOperationOptional : PatchOperation
	{
		public string settingName;
		public PatchOperation caseTrue;
		public PatchOperation caseFalse;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (WVC_Ultra.settings.GetEnabledSettings.Contains(settingName) && caseTrue != null)
			{
				return caseTrue.Apply(xml);
			}
			else if (WVC_Ultra.settings.GetEnabledSettings.Contains(settingName) != true && caseFalse != null)
			{
				return caseFalse.Apply(xml);
			}
			return true;
		}
	}
}
