using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_UltraExpansion
{

	public class CompProperties_Spawner : CompProperties
	{
		public IntRange ticksUntilSpawn = new(120000, 360000);

		public List<ThingDef> productDefs;

		public IntRange productCount = new(1, 1);

		public bool writeTimeLeftToSpawn = true;

		public bool requiresPower = false;

		public bool showMessageIfOwned = false;

		public bool spawnForbidden = false;

		public string uniqueTag = "UltraSpawner";

		public CompProperties_Spawner()
		{
			compClass = typeof(CompSpawner);
		}
	}

	public class CompSpawner : ThingComp
	{

		public int ticksUntilSpawn;

		public ThingDef productDef;

		public int productCount = 1;

		public CompProperties_Spawner Props => (CompProperties_Spawner)props;

		// public CompAtomizer Atomizer => parent.TryGetComp<CompAtomizer>();

		public bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				ResetInterval();
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

		public void Tick(int tick)
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
					ResetInterval();
				}
			}
		}

		private void SpawnItems()
		{
			if (productDef == null)
			{
				productDef = Props.productDefs.RandomElement();
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
				Messages.Message("MessageCompSpawnerSpawnedItem".Translate(productDef.LabelCap), thing, MessageTypeDefOf.PositiveEvent);
			}
		}

		private void ResetInterval()
		{
			ticksUntilSpawn = Props.ticksUntilSpawn.RandomInRange;
			productDef = Props.productDefs.RandomElement();
			productCount = Props.productCount.RandomInRange;
			// ticksUntilSpawn = Atomizer.TicksPerAtomize;
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
						ResetInterval();
					}
				};
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref ticksUntilSpawn, "ticksToSpawnThing" + "_" + Props.uniqueTag, 0);
		}

		public override string CompInspectStringExtra()
		{
			if (PowerOn && Props.writeTimeLeftToSpawn)
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(productDef, null, productCount)).Resolve() + ": " + ticksUntilSpawn.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}
	}

}
