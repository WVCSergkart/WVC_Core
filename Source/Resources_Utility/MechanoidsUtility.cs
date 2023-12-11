using RimWorld;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_UltraExpansion
{

	public static class MechanoidsUtility
	{

		public static bool CanInstallImplant(Pawn mech, CompTargetEffect_InstallImplantInTarget install_comp)
		{
			if (MechanoidIsPlayerMechanoid(mech) && CanRecharge(mech, out Need_MechEnergy energy) && (energy.IsSelfShutdown || energy.IsLowEnergySelfShutdown) && HasUnoccupiedBodyParts(mech, install_comp))
			{
				return true;
			}
			return false;
		}

		public static bool HasUnoccupiedBodyParts(Pawn pawn, CompTargetEffect_InstallImplantInTarget install_comp)
		{
			if (install_comp != null)
			{
				List<BodyPartRecord> bodyPartRecords = pawn.RaceProps.body.GetPartsWithDef(install_comp.Props.bodyPart);
				for (int i = 0; i < bodyPartRecords.Count; i++)
				{
					if (bodyPartRecords[i] != null && !pawn.health.hediffSet.HasHediff(install_comp.Props.hediffDef, bodyPartRecords[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CanRecharge(Pawn pawn, out Need_MechEnergy energy)
		{
			energy = pawn?.needs?.energy;
			if (energy == null)
			{
				return false;
			}
			return true;
		}

		// Mech Implants
		public static bool MechanoidIsPlayerMechanoid(Pawn mech)
		{
			if (mech.RaceProps.IsMechanoid 
			&& mech.def.defName.Contains("Mech_") 
			&& mech.kindDef.defName.Contains("Mech_") 
			&& MechDefNameShouldNotContain(mech.def.defName)
			&& MechDefNameShouldNotContain(mech.kindDef.defName)
			&& mech.RaceProps.thinkTreeMain == WVC_UltraDefOf.Mechanoid
			&& mech.RaceProps.thinkTreeConstant == WVC_UltraDefOf.MechConstant
			&& EverControllable(mech.def)
			&& EverRepairable(mech.def)
			&& MechanitorUtility.IsPlayerOverseerSubject(mech)
			&& mech.IsColonyMechPlayerControlled)
			{
				return true;
			}
			return false;
		}

		public static bool MechDefNameShouldNotContain(string defName)
		{
			List<string> list = new();
			foreach (UltraExpansionListDef item in DefDatabase<UltraExpansionListDef>.AllDefsListForReading)
			{
				list.AddRange(item.mechDefNameShouldNotContain);
			}
			if (list.Contains(defName))
			{
				return false;
			}
			return true;
		}

		public static bool EverControllable(ThingDef def)
		{
			List<CompProperties> comps = def.comps;
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].compClass == typeof(CompOverseerSubject))
				{
					return true;
				}
			}
			return false;
		}

		public static bool EverRepairable(ThingDef def)
		{
			List<CompProperties> comps = def.comps;
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].compClass == typeof(CompMechRepairable))
				{
					return true;
				}
			}
			return false;
		}


	}
}
