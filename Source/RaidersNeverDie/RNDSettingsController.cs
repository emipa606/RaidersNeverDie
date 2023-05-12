using Mlie;
using UnityEngine;
using Verse;

namespace RaidersNeverDie;

public class RNDSettingsController : Mod
{
    public static string currentVersion;

    public RNDSettingsController(ModContentPack content) : base(content)
    {
        GetSettings<RNDSettings>();
        currentVersion =
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