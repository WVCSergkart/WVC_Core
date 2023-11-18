using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_UltraExpansion
{

	public class ThingDefByWeight
	{

		public ThingDef productDef;

		public IntRange productCount = new(1, 1);

		public float selectionWeight = 1.0f;

	}

	public class CompProperties_Spawner : CompProperties
	{
		public IntRange ticksUntilSpawn = new(120000, 360000);

		// public List<ThingDef> productDefs;

		public List<ThingDefByWeight> productDefs;

		// public IntRange productCount = new(1, 1);

		public bool writeTimeLeftToSpawn = true;

		public bool requiresPower = false;

		public bool showMessageIfOwned = false;

		public bool spawnForbidden = false;

		public string uniqueTag = "UltraSpawner";

		public string spawnMessage = "MessageCompSpawnerSpawnedItem";

		public CompProperties_Spawner()
		{
			compClass = typeof(CompSpawner);
		}
	}

	public class CompSpawner : ThingComp
	{

		public int ticksUntilSpawn = 1500;

		public ThingDef productDef;

		public int productCount = 1;

		public ThingDefByWeight thingDefByWeight;

		public CompProperties_Spawner Props => (CompProperties_Spawner)props;

		// public CompAtomizer Atomizer => parent.TryGetComp<CompAtomizer>();

		public bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				Reset();
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
				Reset();
			}
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

		private void Reset()
		{
			ticksUntilSpawn = Props.ticksUntilSpawn.RandomInRange;
			thingDefByWeight = Props.productDefs.RandomElementByWeight((ThingDefByWeight x) => x.selectionWeight);
			productDef = thingDefByWeight.productDef;
			productCount = thingDefByWeight.productCount.RandomInRange;
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
