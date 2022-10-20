using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace RaidersNeverDie;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmony = new Harmony("rimworld.schplorg.raidersneverdie");
        // CheckForStateChange Patch
        SuperPatch(harmony, typeof(Pawn_HealthTracker), "CheckForStateChange", "CheckForStateChange_Prefix");
    }

    private static void SuperPatch(Harmony harmony, Type t, string a, string b)
    {
        var targetmethod = AccessTools.Method(t, a);
        var prefixmethod = new HarmonyMethod(typeof(HarmonyPatches).GetMethod(b));
        harmony.Patch(targetmethod, prefixmethod);
    }

    public static bool CheckForStateChange_Prefix(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff)
    {
        var traversed = Traverse.Create(__instance);
        var pawn = traversed.Field("pawn").GetValue<Pawn>();
        var forceIncap = traversed.Field("forceIncap").GetValue<bool>();
        var shouldBeDead = traversed.Method("ShouldBeDead").GetValue<bool>();
        var shouldBeDowned = traversed.Method("ShouldBeDowned").GetValue<bool>();
        if (__instance.Dead)
        {
            return false;
        }

        if (shouldBeDead)
        {
            if (!pawn.Destroyed)
            {
                pawn.Kill(dinfo, hediff);
            }

            return false;
        }

        if (!__instance.Downed)
        {
            if (shouldBeDowned)
            {
                if (!forceIncap && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(pawn) &&
                    !pawn.IsWildMan() && pawn.Faction is not { IsPlayer: true } &&
                    pawn.HostFaction is not { IsPlayer: true })
                {
                    var animalChance = RNDSettings.animalDeaths * 0.5f;
                    var raiderChance = RNDSettings.raiderDeaths *
                                       (HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve
                                            .Evaluate(StorytellerUtilityPopulation.PopulationIntent) *
                                        Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor);
                    var mechanoidChance = RNDSettings.mechanoidDeaths;
                    var chance = pawn.RaceProps.Animal ? animalChance :
                        !pawn.RaceProps.IsMechanoid ? raiderChance : mechanoidChance;
                    if (Rand.Chance(chance))
                    {
                        pawn.Kill(dinfo);
                        return false;
                    }
                }

                Type[] t = { typeof(DamageInfo?), typeof(Hediff) };
                object[] ps = { dinfo, hediff };
                traversed.Method("MakeDowned", t, ps).GetValue();
                return false;
            }

            if (__instance.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                return false;
            }

            if (pawn.carryTracker?.CarriedThing != null && pawn.jobs != null && pawn.CurJob != null)
            {
                pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            }

            if (pawn.equipment?.Primary == null)
            {
                return false;
            }

            if (pawn.kindDef.destroyGearOnDrop)
            {
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
            }
            else if (pawn.InContainerEnclosed)
            {
                pawn.equipment.TryTransferEquipmentToContainer(pawn.equipment.Primary, pawn.holdingOwner);
            }
            else if (pawn.SpawnedOrAnyParentSpawned)
            {
                pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out _, pawn.PositionHeld);
            }
            else
            {
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
            }

            return false;
        }

        if (shouldBeDowned)
        {
            return false;
        }

        traversed.Method("MakeUndowned").GetValue();
        return false;
    }
}