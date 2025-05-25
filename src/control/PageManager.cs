using UnityEngine;
using BepInEx.Configuration;

using SlugArchive.src.view;

namespace SlugArchive.src.control
{
    public class PageManager : MonoBehaviour
    {
        private readonly PageBase PB;
        private readonly KeyboardShortcut escapeKey = new(KeyCode.Escape);
        private KeyboardShortcut toggleKey;
        private bool isVisible;
        private bool paused;

        public PageManager()
        {
            isVisible = false;
            paused = false;
            toggleKey = new(KeyCode.BackQuote);

            // To create GameObject & add componet PageBase
            GameObject pageObject = new("SlugArchivePage");
            PB = pageObject.AddComponent<PageBase>();
        }

        public void Update(On.RainWorld.orig_Update orig, RainWorld self)
        {
            // To block update when paused
            if (SlugArchiveMain.RWG == null)
            {
                orig(self);
                return;
            }
            if (!paused || !isVisible)
            {
                orig(self);
            }

            if (toggleKey.MainKey != ConfigInterface.keyBind.Value)
            {
                toggleKey = new KeyboardShortcut(ConfigInterface.keyBind.Value);
            }

            if (paused)
            {
                if (toggleKey.IsDown() || escapeKey.IsDown())
                {
                    ShowPage(-1);
                }
            }
            else
            {
                if (toggleKey.IsDown()) ShowPage(0);
            }
        }

        public void SetGamePaused(bool pause)
        {
            paused = pause && ConfigInterface.autoPause.Value;
        }

        public void ShowPage(int page)
        {
            if (page == -1)
            {
                SetGamePaused(false);
                isVisible = false;
                PB.Hide();
                Cursor.visible = false;
            }
            else
            {
                SetGamePaused(true);
                isVisible = true;
                PB.Show();
                Cursor.visible = true;
            }
        }
    }
}
