using MonoMod.RuntimeDetour;
using System.Reflection;

namespace SlugArchive.src.control;

public class HooksDistributor
{
    public static Hook ShowCursorHook;

    public static void Distribute()
    {
        On.RainWorld.Update += SlugArchiveMain.PM.Update;

        //change visibility Rain World cursor
        ShowCursorHook = new Hook(
            typeof(Menu.Menu).GetProperty("ShowCursor", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(),
            typeof(HooksDistributor).GetMethod("Menu_ShowCursor_get", BindingFlags.Static | BindingFlags.Public)
        );
    }

    public delegate bool orig_ShowCursor(Menu.Menu self);
    public static bool Menu_ShowCursor_get(orig_ShowCursor orig, Menu.Menu self)
    {
        bool ret = orig(self);
        //if (Options.disVnlCursor?.Value == true)
        //    ret = false;
        return ret;
    }
}
