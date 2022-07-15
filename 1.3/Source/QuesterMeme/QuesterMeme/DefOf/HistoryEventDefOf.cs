using System;
using RimWorld;

namespace QuesterMeme
{
	[DefOf]
	public static class HistoryEventDefOf
	{
		static HistoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HistoryEventDefOf));
		}

		public static HistoryEventDef QuesterMeme_QuestComplete_Success;
		public static HistoryEventDef QuesterMeme_QuestComplete_DangerousSuccess;
		public static HistoryEventDef QuesterMeme_QuestComplete_Fail;
		public static HistoryEventDef QuesterMeme_QuestComplete_FailAutoAccept;
		public static HistoryEventDef QuesterMeme_QuestExpired;
	}
}