using RimWorld;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_UltraExpansion
{

	public static class MiscUtility
	{

		public static void InstallPartTo(Pawn mech, BodyPartRecord part, HediffDef hediffDef, ThingDef moteDef, Thing item)
		{
			Hediff hediff = HediffMaker.MakeHediff(hediffDef, mech);
			mech.health.AddHediff(hediff, part);
			SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(mech));
			ThingDef thingDef = moteDef;
			if (thingDef != null)
			{
				MoteMaker.MakeAttachedOverlay(mech, thingDef, Vector3.zero);
			}
			item.SplitOff(1).Destroy();
			Messages.Message("WVC_Ultra_MechImplantInstalled".Translate(hediffDef.label.CapitalizeFirst(), mech.LabelCap), mech, MessageTypeDefOf.PositiveEvent, historical: false);
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
