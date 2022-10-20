using UnityEngine;
using Verse;

namespace RaidersNeverDie;

public class RNDSettings : ModSettings
{
    public static float raiderDeaths;
    public static float mechanoidDeaths = 1.0f;
    public static float animalDeaths = 1.0f;

    public void DoWindowContents(Rect inRect)
    {
        var viewRect = new Rect(0f, 0f, inRect.width, inRect.height);
        var listingStandard = new Listing_Standard { maxOneColumn = true, ColumnWidth = viewRect.width };

        listingStandard.Gap(40f);
        listingStandard.Label("RND.raiderDeaths".Translate(raiderDeaths.ToStringPercent()));
        raiderDeaths = listingStandard.Slider(raiderDeaths, 0.0f, 10.0f);
        listingStandard.Label("RND.mechanoidDeaths".Translate(mechanoidDeaths.ToStringPercent()));
        mechanoidDeaths = listingStandard.Slider(mechanoidDeaths, 0.0f, 10.0f);
        listingStandard.Label("RND.animalDeaths".Translate(animalDeaths.ToStringPercent()));
        animalDeaths = listingStandard.Slider(animalDeaths, 0.0f, 10.0f);
        if (RNDSettingsController.currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RND.ModVersion".Translate(RNDSettingsController.currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref raiderDeaths, "raiderDeaths", 0.0f, true);
        Scribe_Values.Look(ref mechanoidDeaths, "mechanoidDeaths", 1.0f, true);
        Scribe_Values.Look(ref animalDeaths, "animalDeaths", 1.0f, true);
    }
}