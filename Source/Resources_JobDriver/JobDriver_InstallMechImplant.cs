using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace WVC_UltraExpansion
{

	public class CompProperties_TargetEffect_InstallImplantInTarget : CompProperties
	{
		public JobDef jobDef;

		public ThingDef moteDef;

		public HediffDef hediffDef;

		public BodyPartDef bodyPart;

		public CompProperties_TargetEffect_InstallImplantInTarget()
		{
			compClass = typeof(CompTargetEffect_InstallImplantInTarget);
		}
	}

	public class CompTargetEffect_InstallImplantInTarget : CompTargetEffect
	{
		// public XenotypeDef xenotypeDef = null;

		public CompProperties_TargetEffect_InstallImplantInTarget Props => (CompProperties_TargetEffect_InstallImplantInTarget)props;

		// public override void PostPostMake()
		// {
			// base.PostPostMake();
			// if (xenotypeDef == null)
			// {
				// if (Props.xenotypeDef != null)
				// {
					// xenotypeDef = Props.xenotypeDef;
				// }
				// else
				// {
					// List<XenotypeDef> xenotypeDefs = XenotypeFilterUtility.WhiteListedXenotypes(true, true);
					// xenotypeDef = xenotypeDefs.RandomElement();
				// }
			// }
		// }

		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
			{
				Job job = JobMaker.MakeJob(Props.jobDef, target, parent);
				job.count = 1;
				user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				// Pawn innerPawn = ((Corpse)target.Thing).InnerPawn;
			}
		}

		// public override string TransformLabel(string label)
		// {
			// if (xenotypeDef == null)
			// {
				// return parent.def.label + " (" + "ERR" + ")";
			// }
			// return parent.def.label + " (" + xenotypeDef.label + ")";
		// }

		// public override void PostExposeData()
		// {
			// base.PostExposeData();
			// Scribe_Defs.Look(ref xenotypeDef, "xenotypeDef");
		// }
	}

	public class JobDriver_InstallMechImplant : JobDriver
	{
		// private const TargetIndex CorpseInd = TargetIndex.A;

		// private const TargetIndex ItemInd = TargetIndex.B;

		// private const int DurationTicks = 600;

		private Mote warmupMote;

		// private Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

		private Pawn Pawn => (Pawn)job.GetTarget(TargetIndex.A);

		private Thing Item => job.GetTarget(TargetIndex.B).Thing;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (pawn.Reserve(Pawn, job, 1, -1, null, errorOnFailed))
			{
				return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
			}
			return false;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600);
			toil.WithProgressBarToilDelay(TargetIndex.A);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate
			{
				CompUsable compUsable = Item.TryGetComp<CompUsable>();
				if (compUsable != null && warmupMote == null && compUsable.Props.warmupMote != null)
				{
					warmupMote = MoteMaker.MakeAttachedOverlay(Pawn, compUsable.Props.warmupMote, Vector3.zero);
				}
				warmupMote?.Maintain();
			};
			yield return toil;
			yield return Toils_General.Do(Install);
		}

		private void Install()
		{
			CompTargetEffect_InstallImplantInTarget install_comp = Item?.TryGetComp<CompTargetEffect_InstallImplantInTarget>();
			if (install_comp == null)
			{
				return;
			}
			if (install_comp.Props.bodyPart == null || install_comp.Props.hediffDef == null)
			{
				return;
			}
			List<BodyPartRecord> bodyPartRecords = Pawn.RaceProps.body.GetPartsWithDef(install_comp.Props.bodyPart);
			for (int i = 0; i < bodyPartRecords.Count; i++)
			{
				if (bodyPartRecords[i] != null && !Pawn.health.hediffSet.HasHediff(install_comp.Props.hediffDef, bodyPartRecords[i]))
				{
					ThingDef oldPart = GetFirstHediffOnPart(Pawn.health.hediffSet.hediffs, bodyPartRecords[i])?.def?.spawnThingOnRemoved;
					if (oldPart != null)
					{
						Thing thing = ThingMaker.MakeThing(oldPart);
						thing.stackCount = 1;
						GenPlace.TryPlaceThing(thing, Pawn.Position, Pawn.Map, ThingPlaceMode.Near, out var lastResultingThing, null, default);
					}
					// Log.Error(bodyPartRecords[i].Label);
					Hediff hediff = HediffMaker.MakeHediff(install_comp.Props.hediffDef, Pawn);
					Pawn.health.AddHediff(hediff, bodyPartRecords[i]);
					SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(Pawn));
					ThingDef thingDef = install_comp.Props.moteDef;
					if (thingDef != null)
					{
						MoteMaker.MakeAttachedOverlay(Pawn, thingDef, Vector3.zero);
					}
					Item.SplitOff(1).Destroy();
					Messages.Message("WVC_Ultra_MechImplantInstalled".Translate(install_comp.Props.hediffDef.label.CapitalizeFirst(), Pawn.LabelCap), Pawn, MessageTypeDefOf.PositiveEvent, historical: false);
					break;
				}
			}
		}

		public static Hediff GetFirstHediffOnPart(List<Hediff> hediffs, BodyPartRecord part)
		{
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Part == part)
				{
					return hediffs[i];
				}
			}
			return null;
		}
	}

}
