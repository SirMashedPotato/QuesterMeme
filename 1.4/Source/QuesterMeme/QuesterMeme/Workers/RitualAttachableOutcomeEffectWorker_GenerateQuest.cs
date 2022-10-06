using System.Collections.Generic;
using Verse;
using RimWorld;
namespace QuesterMeme
{
    class RitualAttachableOutcomeEffectWorker_GenerateQuest : RitualAttachableOutcomeEffectWorker
	{
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = this.def.letterInfoText;
			GenerateQuest(jobRitual);
			if (outcome.BestPositiveOutcome(jobRitual))
            {
				GenerateQuest(jobRitual);
			}
		}

		private void GenerateQuest(LordJob_Ritual jobRitual)
        {
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				points = StorytellerUtility.DefaultThreatPointsNow(jobRitual.Map)
			};
			IncidentDefOf.GiveQuest_Random.Worker.TryExecute(parms);
        }
	}
}
