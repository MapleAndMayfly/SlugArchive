using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using System.Text;

using SlugArchive.src.view;
using UnityEngine;

#pragma warning disable CS0618
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace SlugArchive.src.control;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class SlugArchiveMain : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "kaede.slug_archive";
    public const string PLUGIN_NAME = "Slug Archive";
    public const string PLUGIN_VERSION = "0.1.0";
    public const string PLUGIN_DIR = "slugarchive";

    public static PageManager PM { get; private set; }
    public static KKModTranslator KKMT { get; private set; }
    public static SlugArchiveMain Instance { get; private set; }
    public static RainWorldGame RWG { get; private set; }
    private static ConfigInterface config;
    private static bool IsInited;

    private void OnEnable()
    {
        Instance = this;
        On.RainWorld.OnModsInit += Init;
    }

    private void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        KKMT ??= new(PLUGIN_DIR);
        MachineConnector.SetRegisteredOI(PLUGIN_GUID, config = new());

        if (IsInited) return;
        try
        {
            IsInited = true;

            On.RainWorldGame.ctor += (orig, self, manager) =>
            {
                orig(self, manager);
                RWG = self;
            };
            On.RainWorldGame.ExitGame += (orig, self, asDeath, asQuit) =>
            {
                orig(self, asDeath, asQuit);
                RWG = null;
            };

            PM = new();
            HooksDistributor.Distribute();

            Log(this, $"Plugin [{PLUGIN_NAME}] initialized successfully.");
        }
        catch (Exception ex)
        {
            Err(this, ex);
        }
    }

    public void Log(object selfClass, string msg)
    {
        StringBuilder sb = new();
        sb.AppendLine("\n\t================================================================")
          .AppendLine( $"\tMessage from [{selfClass.GetType().Name}]:")
          .AppendLine(  "\t----------------------------------------------------------------").Append("\t")
          .AppendLine(     msg)
          .AppendLine("\n\t================================================================\n");

        Logger.LogMessage(sb.ToString());
    }
    public void Warn(object selfClass, string warning)
    {
        StringBuilder sb = new();
        sb.AppendLine("\n\t================================================================")
          .AppendLine( $"\tWarning from [{selfClass.GetType().Name}]:")
          .AppendLine(  "\t----------------------------------------------------------------").Append("\t")
          .AppendLine(     warning)
          .AppendLine("\n\t================================================================\n");

        Logger.LogWarning(sb.ToString());
    }
    public void Err(object selfClass, Exception ex)
    {
        Logger.LogError("\n================================================================\n" +
                $"Exception occurs at [{selfClass.GetType().Name}]:\n" +
                "----------------------------------------------------------------\n" +
                ex + "\n\n================================================================\n\n");
    }
}
