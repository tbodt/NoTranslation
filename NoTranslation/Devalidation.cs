using COSML;
using COSML.Modding;
using UnityEngine;

namespace NoTranslation
{
    public class Devalidation : Mod, IModTogglable
    {
        public Devalidation() : base("Devalidation") { }

        public override void Init()
        {
            On.JournalKeyUI.CheckClick += JournalKeyUI_CheckClick;
            On.JournalSlotBoxUI.CheckClick += JournalSlotBoxUI_CheckClick;
            On.JournalKeyUI.UnassignKey += JournalKeyUI_UnassignKey;
            On.JournalSlotUI.ChangeState += JournalSlotUI_ChangeState;
            On.JournalKeyUI.GetCursorType += JournalKeyUI_GetCursorType;
            On.JournalSlotBoxUI.GetCursorType += JournalSlotBoxUI_GetCursorType;
        }

        public void Unload()
        {
            On.JournalKeyUI.CheckClick -= JournalKeyUI_CheckClick;
            On.JournalSlotBoxUI.CheckClick -= JournalSlotBoxUI_CheckClick;
            On.JournalKeyUI.UnassignKey -= JournalKeyUI_UnassignKey;
            On.JournalSlotUI.ChangeState -= JournalSlotUI_ChangeState;
        }

        private void JournalKeyUI_CheckClick(On.JournalKeyUI.orig_CheckClick orig, JournalKeyUI self)
        {
            InputsController inputsController = GameController.GetInstance().GetInputsController();
            if (inputsController.OnStartDrag())
            {
                inputsController.EnterDragUI(self);
            }
            else
            {
                orig(self);
            }
        }

        private void JournalSlotBoxUI_CheckClick(On.JournalSlotBoxUI.orig_CheckClick orig, JournalSlotBoxUI self)
        {
            InputsController inputsController = GameController.GetInstance().GetInputsController();
            if (inputsController.OnStartDrag())
            {
                inputsController.EnterDragUI(self);
            }
            else
            {
                orig(self);
            }
        }

        private static string AnimatorCurrentClipName(Animator animator)
        {
            AnimatorClipInfo[] current = animator.GetCurrentAnimatorClipInfo(0);
            return current.Length == 0 ? null : current[0].clip.name;
        }

        private void JournalKeyUI_UnassignKey(On.JournalKeyUI.orig_UnassignKey orig, JournalKeyUI self)
        {
            Animator animator = ReflectionHelper.GetField<JournalKeyUI, Animator>(self, "animator");
            orig(self);
            if (AnimatorCurrentClipName(animator) == "JournalKeyValidatedAnimation")
            {
                animator.Play("Base Layer.Init");
            }
        }

        private void JournalSlotUI_ChangeState(On.JournalSlotUI.orig_ChangeState orig, JournalSlotUI self, int newState)
        {
            Animator animator = ReflectionHelper.GetField<JournalSlotUI, Animator>(self, "slotAnimator");
            orig(self, newState);
            if (AnimatorCurrentClipName(animator) == "JournalSlotValidAnimation")
            {
                animator.Play("Base Layer.Init");
            }
        }

        private InteractiveCursorType JournalSlotBoxUI_GetCursorType(On.JournalSlotBoxUI.orig_GetCursorType orig, JournalSlotBoxUI self)
        {
            if (self.GetSlotUI().GetRuneData() != null && !GameController.GetInstance().GetJournal().WaitValidAnimation())
            {
                return InteractiveCursorType.DRAG;
            }
            return orig(self);
        }

        private InteractiveCursorType JournalKeyUI_GetCursorType(On.JournalKeyUI.orig_GetCursorType orig, JournalKeyUI self)
        {
            if (self.GetRuneData().GetState() == RuneStateData.VALIDATED)
            {
                return InteractiveCursorType.BUTTON;
            }
            return orig(self);
        }

    }
}

