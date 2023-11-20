using RimWorld;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Verse;
using WVC_UltraExpansion.ImplantGenerator;

namespace WVC_UltraExpansion
{

	[StaticConstructorOnStartup]
	public static class PostInitializationDefGenerator
	{
		public static List <ThingDef> thingDefs = new();
		public static List <RecipeDef> recipeDefs = new();
		public static List <HediffDef> hediffDefs = new();

		static PostInitializationDefGenerator()
		{
			bool onOrOff = false;
			if (onOrOff)
			{
				TemplatesUtility.GetOrCreateStorageModFolders(out string path);
				TemplatesUtility.GetOrCreateStorageModFolders(out string path1);
				TemplatesUtility.GetOrCreateStorageModFolders(out string path2);
				foreach (ImplantGeneratorDef generatorDef in DefDatabase<ImplantGeneratorDef>.AllDefsListForReading)
				{
					// string name = "DeleteTag";
					XDocument xDocument1 = new(new XElement("Defs"));
					XDocument xDocument2 = new(new XElement("Defs"));
					XDocument xDocument3 = new(new XElement("Defs"));
					// xDocument1.Element("Defs").SetAttributeValue(name, "DeleteImplant_" + generatorDef.defName);
					// xDocument2.Element("Defs").SetAttributeValue(name, "DeleteImplant_" + generatorDef.defName);
					// xDocument3.Element("Defs").SetAttributeValue(name, "DeleteImplant_" + generatorDef.defName);
					foreach (BodyPartDef bodyPartDef in generatorDef.bodyPartDefs)
					{
						TemplatesUtility.GetFromTemplate_ThingDef(generatorDef.thingTemplateDef, bodyPartDef, generatorDef);
					}
					foreach (HediffDef hediffDef in hediffDefs)
					{
						xDocument1.Element("Defs").Add(TemplatesUtility.GenerateHediffDefFile(hediffDef, generatorDef));
					}
					foreach (RecipeDef recipeDef in recipeDefs)
					{
						xDocument2.Element("Defs").Add(TemplatesUtility.GenerateSurgeryDefFile(recipeDef));
					}
					foreach (ThingDef thingDef in thingDefs)
					{
						xDocument3.Element("Defs").Add(TemplatesUtility.GenerateThingDefFile(thingDef, generatorDef));
					}
					xDocument1.Save(Path.Combine(path, "HediffDef_" + generatorDef.defName + ".xml"));
					xDocument2.Save(Path.Combine(path1, "RecipeDef_" + generatorDef.defName + ".xml"));
					xDocument3.Save(Path.Combine(path2, "ThingDef_" + generatorDef.defName + ".xml"));
					thingDefs.Clear();
					recipeDefs.Clear();
					hediffDefs.Clear();
				}
			}
		}
	}

}
