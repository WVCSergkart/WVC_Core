using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace WVC_UltraExpansion
{

	public class CompDrillingRig : ThingComp
	{

		public int ticksUntilSpawn = 1500;

		public ThingDef productDef;

		private List<ThingDef> cachedProductDefs;

		public int productCount = 1;

		public ThingDefByWeight thingDefByWeight;

		public CompProperties_Spawner Props => (CompProperties_Spawner)props;

		// public CompAtomizer Atomizer => parent.TryGetComp<CompAtomizer>();

		public bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			CacheMineableStuff();
			if (!respawningAfterLoad)
			{
				Reset();
			}
		}

		private void CacheMineableStuff()
		{
			cachedProductDefs = new();
			foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (item != null && item.deepCommonality > 0f)
				{
					cachedProductDefs.Add(item);
				}
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			Tick(1);
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			Tick(250);
		}

		public override void CompTickLong()
		{
			base.CompTickRare();
			Tick(2000);
		}

		private void Tick(int tick)
		{
			if (!Props.requiresPower || PowerOn)
			{
				ticksUntilSpawn -= tick;
				if (ticksUntilSpawn <= 0)
				{
					if (parent.Map != null)
					{
						SpawnItems();
					}
					Reset();
				}
			}
		}

		private void SpawnItems()
		{
			if (productDef == null)
			{
				CacheMineableStuff();
				Reset();
			}
			if (productDef == null)
			{
				Log.Error(parent.def.label.CapitalizeFirst() + " " + "mineable stuff is null. This can happen if the chances of ore spawning are too low or zero.");
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(productDef);
				thing.stackCount = productCount;
				GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near, out var lastResultingThing, null, default);
				if (Props.spawnForbidden || parent.Faction != Faction.OfPlayer)
				{
					lastResultingThing.SetForbidden(value: true);
				}
				if (Props.showMessageIfOwned && parent.Faction == Faction.OfPlayer)
				{
					Messages.Message(Props.spawnMessage.Translate(productDef.LabelCap), thing, MessageTypeDefOf.PositiveEvent);
				}
			}
		}

		private void Reset()
		{
			ticksUntilSpawn = Props.ticksUntilSpawn.RandomInRange;
			productDef = cachedProductDefs.RandomElementByWeight((ThingDef x) => x.deepCommonality);
			productCount = (int)(productDef.deepCountPerPortion * Props.productCountFactor);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn thing",
					action = delegate
					{
						SpawnItems();
						Reset();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Get mineable list",
					action = delegate
					{
						CacheMineableStuff();
						if (!cachedProductDefs.NullOrEmpty())
						{
							Log.Error("Mineable stuff:" + "\n" + cachedProductDefs.Select((ThingDef x) => x.defName).ToLineList(" - "));
						}
						else
						{
							Log.Error("Mineable stuff is null");
						}
					}
				};
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref ticksUntilSpawn, "ticksToSpawnThing" + "_" + Props.uniqueTag, 0);
			Scribe_Defs.Look(ref productDef, "productDef" + "_" + Props.uniqueTag);
			Scribe_Values.Look(ref productCount, "productCount" + "_" + Props.uniqueTag);
		}

		public override string CompInspectStringExtra()
		{
			if (PowerOn && Props.writeTimeLeftToSpawn && productDef != null)
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(productDef, null, productCount)).Resolve() + ": " + ticksUntilSpawn.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}
	}

}
