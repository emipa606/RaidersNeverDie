using UnityEngine;
using Verse;

namespace RaidersNeverDie;

public class RNDSettings : ModSettings
{
    public static float RaiderDeaths;
    public static float MechanoidDeaths = 1.0f;
    public static float AnimalDeaths = 1.0f;

    public void DoWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.Label("RND.raiderDeaths".Translate(RaiderDeaths.ToStringPercent()));
        RaiderDeaths = listingStandard.Slider(RaiderDeaths, 0.0f, 1.0f);
        listingStandard.Label("RND.mechanoidDeaths".Translate(MechanoidDeaths.ToStringPercent()));
        MechanoidDeaths = listingStandard.Slider(MechanoidDeaths, 0.0f, 1.0f);
        listingStandard.Label("RND.animalDeaths".Translate(AnimalDeaths.ToStringPercent()));
        AnimalDeaths = listingStandard.Slider(AnimalDeaths, 0.0f, 1.0f);
        if (RNDSettingsController.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RND.ModVersion".Translate(RNDSettingsController.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref RaiderDeaths, "raiderDeaths", 0.0f, true);
        Scribe_Values.Look(ref MechanoidDeaths, "mechanoidDeaths", 1.0f, true);
        Scribe_Values.Look(ref AnimalDeaths, "animalDeaths", 1.0f, true);
    }
}