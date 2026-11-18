using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace MassGravesContinued
{
    public class Building_MassGrave : Building_Grave
    {
        public int CorpseCount => innerContainer.Count;

        public bool CanAcceptCorpses => CorpseCount < Controller.settings.CorpseCapacity;

        public int MaxAssignedPawnsCount => Math.Max(1, Controller.settings.CorpseCapacity - CorpseCount);

        public new bool StorageTabVisible => CanAcceptCorpses;

        private int nextCleanupInterval = -1;

        private const int HalfDayTicks = 30000;

        public override void TickRare()
        {
            base.TickRare();

            if (Controller.settings.EnableAutocleanupCorpse)
            {
                if (Find.TickManager.TicksGame >= nextCleanupInterval)
                {
                    if (!base.HasCorpse || (base.HasCorpse && CorpseCount == Controller.settings.CorpseCapacity))
                    {
                        DeleteCorpses();
                    }
                    nextCleanupInterval += (int) (HalfDayTicks * Controller.settings.CleanupDaysInterval);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref nextCleanupInterval, "nextCleanupInterval", -1);
        }

        public override bool Accepts(Thing thing)
        {
            if (innerContainer.CanAcceptAnyOf(thing) && base.GetStoreSettings().AllowedToAccept(thing))
            {
                return CanAcceptCorpses;
            }
            return false;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(InspectStringPartsFromComps());
            stringBuilder.Append(Translator.Translate("MassGrave_Capacity") + ": " + CorpseCount + "/" + Controller.settings.CorpseCapacity);
            return stringBuilder.ToString();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            IEnumerable<Gizmo> gizmos = base.GetGizmos();
            if (!base.StorageTabVisible && StorageTabVisible)
            {
                foreach (Gizmo item in StorageSettingsClipboard.CopyPasteGizmosFor(base.GetStoreSettings()))
                {
                    yield return item;
                }
            }

            foreach (Gizmo item2 in gizmos)
            {
                if ((item2 as Command_Action)?.defaultLabel != Translator.Translate("CommandGraveAssignColonistLabel"))
                {
                    yield return item2;
                }
            }

            if (Controller.settings.EnableDestroyButton)
            {
                Command_Action command_ActionDelete = new Command_Action();
                command_ActionDelete.defaultLabel = Translator.Translate("CommandMassGraveDestroyCorpsesLabel");
                command_ActionDelete.defaultDesc = Translator.Translate("CommandMassGraveDestroyCorpsesDesc");
                command_ActionDelete.icon = (Texture)(object)ContentFinder<Texture2D>.Get("UI/Buttons/Delete");
                command_ActionDelete.action = delegate
                {
                    DeleteCorpses();
                };
                if (!base.HasCorpse || (base.HasCorpse && CorpseCount != Controller.settings.CorpseCapacity) && !Controller.settings.AlwaysAllowDestroyButton)
                {
                    command_ActionDelete.Disable(Translator.Translate("CommandMassGraveInsufficientCount"));
                }
                yield return command_ActionDelete;
            }
        }

        public void DeleteCorpses()
        {
            if (HasAnyContents)
            {
                this.innerContainer.ClearAndDestroyContents();
                DirtyMapMesh(base.Map);
            }
        }
    }
}
