using Mlie;
using UnityEngine;
using Verse;

namespace RaidersNeverDie;

public class RNDSettingsController : Mod
{
    public static string CurrentVersion;

    public RNDSettingsController(ModContentPack content) : base(content)
    {
        GetSettings<RNDSettings>();
        CurrentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        GetSettings<RNDSettings>().DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Raiders Never Die";
    }
}