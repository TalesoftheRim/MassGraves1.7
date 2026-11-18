using HarmonyLib;
using System.Reflection;
using Verse;

namespace MassGravesContinued
{
    [StaticConstructorOnStartup]
    public static class HarmonyLoader
    {
        static HarmonyLoader()
        {
            var harmony = new Harmony("DimonSever000.MassGraves");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
