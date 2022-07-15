using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;

namespace QuesterMeme
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.SirMashedPotato.QuesterMeme");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void ApplyHistoryEvent(HistoryEventDef eventDef)
        {
            List<Pawn> playerPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            if (!playerPawns.NullOrEmpty())
            {
                foreach (Pawn p in playerPawns)
                {
                    HistoryEvent historyEvent = new HistoryEvent(eventDef, p.Named(HistoryEventArgsNames.Doer));
                    Find.HistoryEventsManager.RecordEvent(historyEvent, true);
                }
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(eventDef, playerPawns[0].Faction.ideos.PrimaryIdeo.Named(HistoryEventArgsNames.Ideo)), true);
            }
        }

        [HarmonyPatch(typeof(Quest))]
        [HarmonyPatch("End")]
        public static class Quest_End_Patch
        {
            [HarmonyPostfix]
            public static void QuesterMeme_Patch(QuestEndOutcome outcome, ref Quest __instance)
            {
                if (ModsConfig.IdeologyActive && !__instance.hidden && !__instance.hiddenInUI)
                {
                    HistoryEventDef eventDef = null;

                    if (outcome == QuestEndOutcome.Success)
                    {
                        if (__instance.challengeRating >= 3)
                        {
                            eventDef = HistoryEventDefOf.QuesterMeme_QuestComplete_DangerousSuccess;
                        } 
                        else
                        {
                            eventDef = HistoryEventDefOf.QuesterMeme_QuestComplete_Success;
                        }
                    }
                    if (outcome == QuestEndOutcome.Fail)
                    {
                        if (__instance.root.autoAccept)
                        {
                            eventDef = HistoryEventDefOf.QuesterMeme_QuestComplete_FailAutoAccept;
                        }
                        else
                        {
                            eventDef = HistoryEventDefOf.QuesterMeme_QuestComplete_Fail;
                        }
                    }
                    if(!__instance.EverAccepted && __instance.State == QuestState.EndedOfferExpired)
                    {
                        eventDef = HistoryEventDefOf.QuesterMeme_QuestExpired;
                    }

                    if (eventDef != null)
                    {
                        ApplyHistoryEvent(eventDef);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Quest))]
        [HarmonyPatch("CleanupQuestParts")]
        public static class Quest_CleanupQuestParts_Patch
        {
            [HarmonyPostfix]
            public static void QuesterMeme_Patch(ref Quest __instance)
            {
                if (ModsConfig.IdeologyActive)
                {
                    if (!__instance.EverAccepted && __instance.State == QuestState.EndedOfferExpired)
                    {
                        ApplyHistoryEvent(HistoryEventDefOf.QuesterMeme_QuestExpired);
                    }
                }
            }
        }
    }
}
