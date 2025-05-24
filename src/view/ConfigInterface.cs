using Menu.Remix.MixedUI;
using SlugArchive.src.control;
using UnityEngine;

namespace SlugArchive.src.view;

public class ConfigInterface : OptionInterface
{
    private readonly KKModTranslator KKMT = SlugArchiveMain.KKMT;

    public static Configurable<bool> autoPause;
    public static Configurable<bool> needNeuron;    // NSHSwarmer or SSOracleSwarmer
    public static Configurable<KeyCode> keyBind;

    private static Vector2 TitlePos { get; } = new(200, 530);
    private static Vector2 SubtitlePos { get; } = new(200, 510);
    private static Vector2 TitleSize { get; } = new(200, 50);
    private static Vector2 ColumSize { get; } = new(150, 50);
    private static Vector2 ListPos { get; } = new(100, 450);
    private static Vector2 CheckBoxSize { get; } = new(130, 30);
    private const float gap = 70f;

    public ConfigInterface()
    {
        autoPause = config.Bind(key: "kaede_SA_autoPause", defaultValue: true);
        needNeuron = config.Bind(key: "kaede_SA_needNeuron", defaultValue: true);
        keyBind = config.Bind(key: "kaede_SA_keyBind", defaultValue: KeyCode.Backslash);
    }

    public override void Initialize()
    {
        float currY = ListPos.y;
        float listX = ListPos.x;

        Tabs = [ new OpTab(this) ];

        // Header
        Tabs[0].AddItems(
            new OpLabel(TitlePos, TitleSize, KKMT.Translate("text.modName"), bigText: true),
            new OpLabel(SubtitlePos, TitleSize, $"ver. {SlugArchiveMain.PLUGIN_VERSION}") { color = Color.gray }
        );

        // AutoPause
        Tabs[0].AddItems(
            new OpLabel(new Vector2(listX, currY), ColumSize, KKMT.Translate("text.autoPause"), bigText: true)
            {
                alignment = FLabelAlignment.Right,
                verticalAlignment = OpLabel.LabelVAlignment.Center,
                description = KKMT.Translate("descript.autoPause")
            },
            new OpCheckBox(autoPause, new Vector2(listX + ColumSize.x + 20, currY + 10))
        );

        // NeedNeuron
        currY -= gap;
        Tabs[0].AddItems(
            new OpLabel(new Vector2(listX, currY), ColumSize, KKMT.Translate("text.needNeuron"), bigText: true)
            {
                alignment = FLabelAlignment.Right,
                verticalAlignment = OpLabel.LabelVAlignment.Center,
                description = KKMT.Translate("descript.needNeuron")
            },
            new OpCheckBox(needNeuron, new Vector2(listX + ColumSize.x + 20, currY + 10))
        );

        // KeyBind
        currY -= gap;
        Tabs[0].AddItems(
            new OpLabel(new Vector2(listX, currY), ColumSize, KKMT.Translate("text.keyBind"), bigText: true)
            {
                alignment = FLabelAlignment.Right,
                verticalAlignment = OpLabel.LabelVAlignment.Center,
                description = KKMT.Translate("descript.keyBind")
            },
            new OpKeyBinder(keyBind, new Vector2(listX + ColumSize.x + 20, currY + 10), CheckBoxSize, false)
        );
        
    }
}
