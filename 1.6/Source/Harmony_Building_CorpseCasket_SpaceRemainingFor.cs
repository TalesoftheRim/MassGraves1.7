using HarmonyLib;
using RimWorld;

namespace MassGravesContinued
{
    [HarmonyPatch(typeof(Building_CorpseCasket), "SpaceRemainingFor")]
    public static class Harmony_Building_CorpseCasket_SpaceRemainingFor
    {
        public static bool Prefix(Building_CorpseCasket __instance, ref int __result)
        {
            if (__instance is Building_MassGrave building_MassGrave)
            {
                __result = 0;
                if (building_MassGrave.CanAcceptCorpses)
                {
                    __result = 1;
                }
                return false;
            }
            return true;
        }
    }
}