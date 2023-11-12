using COSML.Modding;
using System.Collections.Generic;
using static COSML.Menu.MenuUtils;
using System;
using COSML;

namespace NoTranslation
{
    public class NoTranslation : Mod, ILocalSettings<LocalData>, IModTogglable, IModMenu
    {
        private GlobalData globalData = new GlobalData();
        private LocalData localData = new LocalData();

        public NoTranslation() : base("No Translation") { }
        public override string GetVersion() => "1.0.0";
        public void OnLoadGlobal(GlobalData data) => globalData = data;
        public GlobalData OnSaveGlobal() => globalData;
        public void OnLoadLocal(LocalData data) => localData = data;
        public LocalData OnSaveLocal() => localData;

        public override void Init()
        {
            Info("Loaded NoTranslation");
            On.JournalLanguageUI.JournalPage.TryValidPage += Hook_JournalPage_TryValidPage;
            On.PageRevelator.ForceRevelated += Hook_PageRevelator_ForceRevelated;
            On.PageRevelator.GetState += Hook_PageRevelator_GetState;
            On.LanguageController.HasValidatedRunes += Hook_LanguageController_HasValidatedRunes;
            On.LanguageController.HasValidatedLinkedRunes += Hook_LanguageController_HasValidatedLinkedRunes;
        }

        public void Unload()
        {
            Info("Unloaded NoTranslation");
            On.JournalLanguageUI.JournalPage.TryValidPage -= Hook_JournalPage_TryValidPage;
            On.PageRevelator.ForceRevelated -= Hook_PageRevelator_ForceRevelated;
            On.PageRevelator.GetState -= Hook_PageRevelator_GetState;
            On.LanguageController.HasValidatedRunes -= Hook_LanguageController_HasValidatedRunes;
            On.LanguageController.HasValidatedLinkedRunes -= Hook_LanguageController_HasValidatedLinkedRunes;
        }

        private int Hook_PageRevelator_GetState(On.PageRevelator.orig_GetState orig, PageRevelator self)
        {
            int state = orig(self);
            if (state == 1)
            {
                state = 2;
            }
            return state;
        }

        private void Hook_PageRevelator_ForceRevelated(On.PageRevelator.orig_ForceRevelated orig, PageRevelator self) { }
        private void Hook_JournalPage_TryValidPage(On.JournalLanguageUI.JournalPage.orig_TryValidPage orig, JournalLanguageUI.JournalPage self) { }
        private bool Hook_LanguageController_HasValidatedRunes(On.LanguageController.orig_HasValidatedRunes orig, LanguageController self)
        {
            return true;
        }
        private bool Hook_LanguageController_HasValidatedLinkedRunes(On.LanguageController.orig_HasValidatedLinkedRunes orig, LanguageController self)
        {
            return true;
        }

        public List<IOptionData> GetMenu()
        {
            return new List<IOptionData>
            {
            };
        }
    }
}
