using System.Reflection;
using HarmonyLib;
using Verse;

namespace RaidersNeverDie;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("rimworld.schplorg.raidersneverdie").PatchAll(Assembly.GetExecutingAssembly());
    }
}