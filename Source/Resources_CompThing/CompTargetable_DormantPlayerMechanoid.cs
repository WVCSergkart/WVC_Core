using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_UltraExpansion
{

	public class CompTargetable_DormantPlayerMechanoid : CompTargetable
	{
		protected override bool PlayerChoosesTarget => true;

		// public CompTargetEffect_InstallImplantInTarget install_comp = parent?.TryGetComp<CompTargetEffect_InstallImplantInTarget>();

		public CompTargetEffect_InstallImplantInTarget Install_comp => parent.TryGetComp<CompTargetEffect_InstallImplantInTarget>();

		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				canTargetItems = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = (TargetInfo x) => x.Thing is Pawn pawn && ValidateTarget(x.Thing) && Install_comp != null && MechanoidsUtility.CanInstallImplant(pawn, Install_comp)
			};
		}

		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
		}
	}

}
