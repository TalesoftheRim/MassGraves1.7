using HarmonyLib;
using RimWorld;

namespace MassGravesContinued
{
    [HarmonyPatch(typeof(Building_Grave), "StorageTabVisible", MethodType.Getter)]
    public static class Harmony_Storage_Tab_Visible
    {
        public static bool Prefix(Building_Grave __instance, ref bool __result)
        {
            if (__instance is Building_MassGrave building_MassGrave)
            {
                __result = building_MassGrave.StorageTabVisible;
                return false;
            }
            return true;
        }
    }
}
