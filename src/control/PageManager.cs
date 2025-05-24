using UnityEngine;
using BepInEx.Configuration;

using SlugArchive.src.view;

namespace SlugArchive.src.control
{
    public class PageManager : MonoBehaviour
    {
        private readonly KeyboardShortcut escapeKey = new(KeyCode.Escape);
        private KeyboardShortcut toggleKey;
        private bool isVisible;
        private bool paused;

        public PageManager()
        {
            isVisible = false;
            paused = false;
            toggleKey = new(KeyCode.BackQuote);
        }

        public void Update(On.RainWorld.orig_Update orig, RainWorld self)
        {
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

        private void ShowPage(int page)
        {
            if (page == -1)
            {
                SetGamePaused(false);
                isVisible = false;
                Cursor.visible = false;
                // TODO: dispose page
            }
            else
            {
                SetGamePaused(true);
                isVisible = true;
                Cursor.visible = true;
                // TODO: show page
            }
        }
    }
}
