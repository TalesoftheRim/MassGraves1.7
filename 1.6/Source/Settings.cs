using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace MassGravesContinued
{
    public class Settings : ModSettings
    {
        private int corpseCapacity = 20;

        private bool enableDestroyButton;

        private bool alwaysAllowDestroyButton;

        //private bool useAlt;

        private bool enableCorpseAutocleanup;

        private float cleanupDaysInterval = 1f;

        public int CorpseCapacity => corpseCapacity;

        public bool EnableDestroyButton => enableDestroyButton;

        public bool AlwaysAllowDestroyButton => alwaysAllowDestroyButton;

        //public bool UseAlt => useAlt;

        public bool EnableAutocleanupCorpse => enableCorpseAutocleanup;

        public float CleanupDaysInterval => cleanupDaysInterval;

        public string corpseCapacityBuffer;

        public string cleanupDaysBuffer;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref corpseCapacity, "corpseCapacity", 20);
            Scribe_Values.Look(ref enableDestroyButton, "enableDestroyButton", defaultValue: true);
            Scribe_Values.Look(ref alwaysAllowDestroyButton, "alwaysAllowDestroyButton", defaultValue: false);
            Scribe_Values.Look(ref enableCorpseAutocleanup, "enableCorpseAutocleanup", defaultValue: false);
            Scribe_Values.Look(ref cleanupDaysInterval, "cleanupDaysInterval", 1f);
            // Scribe_Values.Look(ref useAlt, "useAlt", defaultValue: false);
        }

        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.ColumnWidth = canvas.width / 1.5f;
            listing_Standard.Begin(canvas);

            if (listing_Standard.ButtonText("MassGrave_DefaultButton".Translate()))
            {
                corpseCapacity = 20;
                
                enableDestroyButton = true;
                alwaysAllowDestroyButton = false;
                enableCorpseAutocleanup = false;

                cleanupDaysInterval = 1f;
            }

            if (corpseCapacityBuffer == null)
            {
                corpseCapacityBuffer = corpseCapacity.ToString("0.#");
            }

            Text.Font = GameFont.Medium;
            listing_Standard.Label(Translator.Translate("MassGrave_Capacity"), -1f, (string)null);
            Text.Font = GameFont.Small;
            //listing_Standard.TextFieldNumericLabeled(Translator.Translate("MassGrave_Capacity"), ref corpseCapacity, ref corpseCapacityBuffer, 1f, 100f);
            listing_Standard.TextFieldNumeric(ref corpseCapacity, ref corpseCapacityBuffer, 1f, 100f);
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled(Translator.Translate("MassGrave_EnableDestroyButton"), ref enableDestroyButton, (string)null);
            listing_Standard.Gap();
            if (enableDestroyButton)
            {
                listing_Standard.CheckboxLabeled(Translator.Translate("MassGrave_AlwaysAllowDestroyButton"), ref alwaysAllowDestroyButton, (string)null);
                listing_Standard.Gap();
            }

            listing_Standard.CheckboxLabeled(Translator.Translate("MassGrave_EnableCorpseAutocleanupButton"), ref enableCorpseAutocleanup, (string)null);
            listing_Standard.Gap();
            if (enableCorpseAutocleanup)
            {
                if (cleanupDaysBuffer == null)
                {
                    cleanupDaysBuffer = cleanupDaysInterval.ToString("0.#");
                }

                listing_Standard.Label(Translator.Translate("MassGrave_CleanupDaysInterval"), -1f, (string)null);
                listing_Standard.TextFieldNumeric(ref cleanupDaysInterval, ref cleanupDaysBuffer, 0.1f, 60f);
                listing_Standard.Gap();

                cleanupDaysInterval = (float)Math.Round(cleanupDaysInterval, 1);
            }
            //listing_Standard.Gap();
            //listing_Standard.CheckboxLabeled(Translator.Translate("MassGrave_UseAlt"), ref useAlt, (string)null);
            listing_Standard.End();
        }

        public void ResetBuffer()
        {
            corpseCapacityBuffer = null;
            cleanupDaysBuffer = null;
        }
    }

}
