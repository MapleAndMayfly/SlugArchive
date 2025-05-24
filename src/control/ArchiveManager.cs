using BepInEx;
using System.IO;
using System.Text;

namespace SlugArchive.src.control;

public class ArchiveManager
{
    private const string SAVE_DIR = "./RainWorld_Data/StreamingAssets/mods/slugarchive/Data";

    private void CheckFileExistence(string file)
    {
        Directory.CreateDirectory(SAVE_DIR);
        string actualPath = new StringBuilder(SAVE_DIR).Append(file).ToString();

        if (!file.IsNullOrWhiteSpace() && !File.Exists(actualPath))
        {
            File.Create(actualPath);
        }
    }
}
