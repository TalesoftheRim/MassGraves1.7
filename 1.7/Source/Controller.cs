using UnityEngine;
using Verse;

namespace MassGravesContinued
{
    public class Controller : Mod
    {
        public static Settings settings;

        public Controller(ModContentPack content)
            : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            return Translator.Translate("MassGrave_Settings");
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            settings.ResetBuffer();

            base.WriteSettings();
        }
    }
}
