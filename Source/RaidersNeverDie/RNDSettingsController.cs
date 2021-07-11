using UnityEngine;
using Verse;

namespace RaidersNeverDie
{
    public class RNDSettingsController : Mod
    {
        public RNDSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<RNDSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<RNDSettings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "RaidersNeverDie";
        }
    }
}