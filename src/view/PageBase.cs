using SlugArchive.src.control;
using UnityEngine;

namespace SlugArchive.src.view;

public class PageBase : MonoBehaviour
{
    private PageManager PM;
    private FContainer container;
    private FSprite background;
    private FSprite[] borders;

    private static readonly Color backColor = new(0f, 0f, 0f, 0.75f);
    private static readonly Color borderColor = new(1f, 1f, 1f, 1f);
    private static readonly Vector2 pageSize = new(600, 750);
    private static readonly float lineWidth = 2;
    private static readonly float pageMargin = 10;

    private void Awake()
    {
        PM = SlugArchiveMain.PM;
        InitializeUI();
    }

    private void InitializeUI()
    {
        container = new();

        float screenMargin = (Futile.screen.pixelHeight - pageSize.y) / 2;
        container.x = Futile.screen.pixelWidth - screenMargin - pageSize.x;
        container.y = screenMargin;
        background = new("pixel")
        {
            anchorX = 0,
            anchorY = 0,
            scaleX = pageSize.x,
            scaleY = pageSize.y,
            color = backColor
        };
        // Four Borders
        borders = [
            // Top
            new FSprite("pixel")
            {
                anchorX = 0,
                anchorY = 0,
                scaleX = pageSize.x + lineWidth * 2 + 1,
                scaleY = lineWidth,
                color = borderColor,
                x = -lineWidth,
                y = pageSize.y
            },
            // Bottom
            new FSprite("pixel")
            {
                anchorX = 0,
                anchorY = 0,
                scaleX = pageSize.x + lineWidth * 2 + 1,
                scaleY = lineWidth,
                color = borderColor,
                x = -lineWidth,
                y = -lineWidth
            },
            // Left
            new FSprite("pixel")
            {
                anchorX = 0,
                anchorY = 0,
                scaleX = lineWidth,
                scaleY = pageSize.y + 1,
                color = borderColor,
                x = -lineWidth,
                y = 0
            },
            // Right
            new FSprite("pixel")
            {
                anchorX = 0,
                anchorY = 0,
                scaleX = lineWidth,
                scaleY = pageSize.y + 1,
                color = borderColor,
                x = pageSize.x,
                y = 0
            }
        ];

        container.AddChild(background);
        foreach (var border in borders)
        {
            container.AddChild(border);
        }

        SlugArchiveMain.Instance.Log(this, "UI initialized sucessfully.");
    }

    public void Show()
    {
        if (container != null)
        {
            if (!Futile.stage._childNodes.Contains(container))
            {
                Futile.stage.AddChild(container);
            }
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (container != null)
        {
            if (Futile.stage._childNodes.Contains(container))
            {
                Futile.stage.RemoveChild(container);
            }
        }
        gameObject.SetActive(false);
    }
}
