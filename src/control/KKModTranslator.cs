using BepInEx;
using Newtonsoft.Json.Linq;
using RWCustom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SlugArchive.src.control;

public class KKModTranslator
{
    private readonly SlugArchiveMain main;
    private readonly StringBuilder langDir = new();
    private string filePath;
    private InGameTranslator.LanguageID currLang;
    private JObject translations;

    public KKModTranslator(string dir)
    {
        main = SlugArchiveMain.Instance;
        // Completing the folder path
        langDir.Append("./RainWorld_Data/StreamingAssets/mods/")
               .Append(dir)
               .Append("/Lang/");
        main.Log(this, $"Lang folder path:\n\t\t{langDir}");

        TryInject();
    }

    private string Direct()
    {
        string ret = InGameTranslator.LanguageID.EncryptIndex(currLang) switch
        {
            9 => "CH",      // Chinese = 9
            _ => "EN"       // English = 0
        };
        return ret + ".json";
    }

    public bool CheckLang()
    {
        if (Custom.rainWorld == null)
        {
            main.Warn(this, "Rainworld object is null.");
            return false;
        }

        if (currLang == null || Custom.rainWorld.options.language != currLang)
        {
            currLang = Custom.rainWorld.options.language;
            filePath = langDir + Direct();
            CheckFileExistence(filePath);

            string jsonData = File.ReadAllText(filePath);
            translations = JObject.Parse(jsonData);
            TryInject();
        }
        return true;
    }

    private void CheckFileExistence(string path)
    {
        ExtEnumInitializer.InitTypes();
        Directory.CreateDirectory(langDir.ToString());

        if (!path.IsNullOrWhiteSpace() && !File.Exists(path))
        {
            main.Err(this, new Exception($"No file located at {path} found!"));
            return;
        }
    }

    public string Translate(string key)
    {
        bool isOk = CheckLang();
        if (!isOk || !translations.ContainsKey(key))
        {
            main.Warn(this, $"No value for key {key} found!");
            return key;
        }
        else return (string)translations[key];
    }

    public void TryInject()
    {
        bool[] found = [false, false];
        bool needRewrite = false;
        string target = "./RainWorld_Data/StreamingAssets/text/" +
                        Translate("file.vanillaTxt") + "/strings.txt";
        if (!File.Exists(target))
        {
            main.Err(main,
                new FileNotFoundException($"File located at [{target}] not found!"));
            return;
        }

        // To check whether or not plugin has injected before
        using (StreamReader sr = new(target))
        {
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(SlugArchiveMain.PLUGIN_GUID + "-name"))
                {
                    found[0] = true;
                }
                else if (line.StartsWith(SlugArchiveMain.PLUGIN_GUID + "-description"))
                {
                    if (!line.Contains(Translate("text.modDescription")))
                    {
                        needRewrite = true;
                        break;
                    }
                    else
                    {
                        found[1] = true;
                        if (found[0]) break;
                    }
                }
            }
        }

        // To remove the description if it changed with plugin updating
        if (needRewrite)
        {
            List<string> list = [.. File.ReadAllLines(target)];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].StartsWith(SlugArchiveMain.PLUGIN_GUID + "-description"))
                {
                    list.RemoveAt(i);
                    found[1] = false;
                    break;
                }
            }
        }

        if (!found[0])
        {
            using StreamWriter sw = new(target, true);
            sw.WriteLine(SlugArchiveMain.PLUGIN_GUID + "-name|" + Translate("text.modName"));
            main.Log(main, "Plugin name injected successfully.");
        }
        if (!found[1])
        {
            using StreamWriter sw = new(target, true);
            sw.WriteLine(SlugArchiveMain.PLUGIN_GUID + "-description|" + Translate("text.modDescription"));
            main.Log(main, "Plugin description injected successfully.");
        }
    }
}
