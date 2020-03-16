using System;
using UnityEngine;
using Verse;

namespace RaidersNeverDie
{
	public class RNDSettings : ModSettings
	{
        public static float raiderDeaths = 0.0f;
        public static float mechanoidDeaths = 1.0f;
        public static float animalDeaths = 1.0f;

		public void DoWindowContents(Rect inRect)
		{
			Rect viewRect = new Rect(0f, 0f, inRect.width, inRect.height);
			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.maxOneColumn = true;
			listingStandard.ColumnWidth = viewRect.width;
			
			listingStandard.Gap(40f);
            listingStandard.Label("raiderDeaths: " + Math.Round(raiderDeaths * 100f, 0) + "%", -1f, "raiderDeaths");
            raiderDeaths = listingStandard.Slider(raiderDeaths, 0.0f, 10.0f);
            listingStandard.Label("mechanoidDeaths: " + Math.Round(mechanoidDeaths * 100f, 0) + "%", -1f, "mechanoidDeaths");
            mechanoidDeaths = listingStandard.Slider(mechanoidDeaths, 0.0f, 10.0f);
            listingStandard.Label("animalDeaths: " + Math.Round(animalDeaths * 100f, 0) + "%", -1f, "animalDeaths");
            animalDeaths = listingStandard.Slider(animalDeaths, 0.0f, 10.0f);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref raiderDeaths, "raiderDeaths", 0.0f, true);
			Scribe_Values.Look<float>(ref mechanoidDeaths, "mechanoidDeaths", 1.0f, true);
			Scribe_Values.Look<float>(ref animalDeaths, "animalDeaths", 1.0f, true);
		}
	}
    public class RNDSettingsController : Mod
    {
        public RNDSettingsController(ModContentPack content) : base(content: content)
        {
            this.GetSettings<RNDSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            this.GetSettings<RNDSettings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory() { 
			return "RaidersNeverDie"; 
		}

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
