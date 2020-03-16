using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using UnityEngine;

namespace RaidersNeverDie
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.schplorg.raidersneverdie");
            // CheckForStateChange Patch
            SuperPatch(harmony, typeof(Verse.Pawn_HealthTracker), "CheckForStateChange", "CheckForStateChange_Prefix");
        }

        private static void SuperPatch(Harmony harmony, Type t, string a, string b)
        {
            MethodInfo targetmethod = AccessTools.Method(t, a);
            HarmonyMethod prefixmethod = new HarmonyMethod(typeof(RaidersNeverDie.HarmonyPatches).GetMethod(b));
            harmony.Patch(targetmethod, prefixmethod, null);
        }

        public static bool CheckForStateChange_Prefix(Verse.Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff)
        {
            var inst = __instance;
            var traversed = Traverse.Create(__instance);
            Pawn pawn = traversed.Field("pawn").GetValue<Pawn>();
            var forceIncap = traversed.Field("forceIncap").GetValue<bool>();
            var shouldBeDead = traversed.Method("ShouldBeDead").GetValue<bool>();
            var shouldBeDowned = traversed.Method("ShouldBeDowned").GetValue<bool>();
            if (inst.Dead)
            {
                return false;
            }
            if (shouldBeDead)
            {
                if (!pawn.Destroyed)
                {
                    pawn.Kill(dinfo, hediff);
                }
            }
            else if (!inst.Downed)
            {
                if (shouldBeDowned)
                {
                    if (!forceIncap && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(pawn) && !pawn.IsWildMan() && (pawn.Faction == null || !pawn.Faction.IsPlayer) && (pawn.HostFaction == null || !pawn.HostFaction.IsPlayer))
                    {
                        float animalChance = RNDSettings.animalDeaths*0.5f;
                        float raiderChance = RNDSettings.raiderDeaths*(HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor);
                        float mechanoidChance = RNDSettings.mechanoidDeaths;
                        float chance = pawn.RaceProps.Animal ? animalChance : ((!pawn.RaceProps.IsMechanoid) ? raiderChance : mechanoidChance);
                        if (Rand.Chance(chance))
                        {
                            pawn.Kill(dinfo);
                            return false;
                        }
                    }
                    forceIncap = false;
                    Type[] t = { typeof(DamageInfo?), typeof(Hediff) };
                    object[] ps = { dinfo, hediff };
                    traversed.Method("MakeDowned", t, ps).GetValue();
                }
                else
                {
                    if (inst.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                    {
                        return false;
                    }
                    if (pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && pawn.jobs != null && pawn.CurJob != null)
                    {
                        pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                    }
                    if (pawn.equipment != null && pawn.equipment.Primary != null)
                    {
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
                            ThingWithComps tmp;
                            pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out tmp, pawn.PositionHeld);
                        }
                        else
                        {
                            pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
                        }
                    }
                }
            }
            else if (!shouldBeDowned)
            {
                traversed.Method("MakeUndowned").GetValue();
            }
            return false;
        }
    }
}