using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace RaidersNeverDie;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.CheckForStateChange))]
public class Pawn_HealthTracker_CheckForStateChange
{
    public static bool Prefix(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff, Pawn ___pawn)
    {
        if (__instance.Dead)
        {
            return false;
        }

        if (ModsConfig.BiotechActive && ___pawn.mechanitor != null)
        {
            ___pawn.mechanitor.Notify_HediffStateChange(hediff);
        }

        if (hediff != null && hediff.def.blocksSleeping && !___pawn.Awake())
        {
            return true;
        }

        if (__instance.ShouldBeDead())
        {
            return true;
        }

        if (!__instance.Downed)
        {
            if (__instance.ShouldBeDowned())
            {
                if (!__instance.forceDowned && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(___pawn) &&
                    !___pawn.IsWildMan() && ___pawn.Faction is not { IsPlayer: true } &&
                    ___pawn.HostFaction is not { IsPlayer: true })
                {
                    var deathless = ModsConfig.BiotechActive &&
                                    ___pawn.genes?.HasActiveGene(GeneDefOf.Deathless) == true;
                    var animalChance = RNDSettings.AnimalDeaths * 0.5f;
                    var raiderChance = RNDSettings.RaiderDeaths *
                                       ((Find.Storyteller.difficulty.unwaveringPrisoners
                                            ? HealthTuning
                                                .DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve
                                            : HealthTuning
                                                .DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners)
                                        .Evaluate(StorytellerUtilityPopulation.PopulationIntent) *
                                        Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor);
                    var mechanoidChance = RNDSettings.MechanoidDeaths;
                    float chance;
                    if (___pawn.RaceProps.Animal)
                    {
                        chance = animalChance;
                    }
                    else if (___pawn.RaceProps.IsMechanoid)
                    {
                        chance = mechanoidChance;
                    }
                    else
                    {
                        chance = raiderChance;
                    }

                    if (Rand.Value <= chance)
                    {
                        if (deathless && !___pawn.Dead)
                        {
                            ___pawn.health.AddHediff(HediffDefOf.MissingBodyPart, ___pawn.health.hediffSet.GetBrain(),
                                dinfo);
                            return false;
                        }

                        ___pawn.Kill(dinfo);
                        return false;
                    }
                }

                AccessTools.Method(typeof(Pawn_HealthTracker), "MakeDowned")
                    .Invoke(__instance, [dinfo, hediff]);
                return false;
            }

            if (__instance.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                return false;
            }

            if (___pawn.carryTracker?.CarriedThing != null && ___pawn.jobs != null && ___pawn.CurJob != null)
            {
                ___pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            }

            if (___pawn.equipment?.Primary == null)
            {
                return false;
            }

            if (___pawn.kindDef.destroyGearOnDrop)
            {
                ___pawn.equipment.DestroyEquipment(___pawn.equipment.Primary);
            }
            else if (___pawn.InContainerEnclosed)
            {
                ___pawn.equipment.TryTransferEquipmentToContainer(___pawn.equipment.Primary, ___pawn.holdingOwner);
            }
            else if (___pawn.SpawnedOrAnyParentSpawned)
            {
                ___pawn.equipment.TryDropEquipment(___pawn.equipment.Primary, out _, ___pawn.PositionHeld);
            }
            else
            {
                ___pawn.equipment.DestroyEquipment(___pawn.equipment.Primary);
            }

            return false;
        }

        if (__instance.ShouldBeDowned())
        {
            return false;
        }

        AccessTools.Method(typeof(Pawn_HealthTracker), "MakeUndowned")
            .Invoke(__instance, [hediff]);
        return false;
    }
}