using AO;
using System;
using System.Collections.Generic;

public static class UIManager
{
    private static bool isUIActive = false;

    struct UIWindow
    {
        public Func<bool> DrawCall;
        public Vector2 Position;
        public bool Positional;
        public int Priority;
    }
    static List<UIWindow> WindowStack = new();

    public static void OpenUI(Func<bool> DrawCall, int priority = 0)
    {
        // Insert higher priority at the end, so they get drawn
        int index = WindowStack.FindIndex(window => window.Priority > priority);
        if (index == -1)
        {
            WindowStack.Add(new UIWindow { DrawCall = DrawCall, Positional = false, Priority = priority });
        }
        else
        {
            WindowStack.Insert(index, new UIWindow { DrawCall = DrawCall, Positional = false, Priority = priority });
        }

        // Set the simple UI active flag too for new UI components
        isUIActive = true;
    }

    public static void OpenUI(System.Action drawFunc)
    {
        if (isUIActive) return;

        isUIActive = true;

        OpenUI(() => { drawFunc(); return true; }, 0);
    }

    public static void OpenPositionalUI(Func<bool> OnDraw, Vector2 position)
    {
        WindowStack.Insert(0, new UIWindow { DrawCall = OnDraw, Positional = true, Position = position });
        isUIActive = true;
    }

    public static void CloseUI()
    {
        if (WindowStack.Count > 0)
        {
            WindowStack.RemoveAt(WindowStack.Count - 1);
        }

        isUIActive = WindowStack.Count > 0;
    }

    public static bool IsUIActive()
    {
        return isUIActive;
    }

    private static void DrawNext(Vector2 playerPos)
    {
        //Close current & draw next if possible
        CloseUI();
        DrawUI(playerPos);
    }

    public static void DrawUI(Vector2 playerPos)
    {
        if (WindowStack.Count == 0) return;

        using var _1 = UI.PUSH_LAYER(10000000);

        for (int i = 0; i < WindowStack.Count - 1; i++)
        {
            var backWindow = WindowStack[i];
            if (backWindow.Positional)
            {
                if (Vector2.Distance(backWindow.Position, playerPos) > 3)
                {
                    continue;
                }
            }
            backWindow.DrawCall();
        }
        var window = WindowStack[^1];
        if (window.Positional)
        {
            if (Vector2.Distance(window.Position, playerPos) > 3)
            {
                DrawNext(playerPos);
                return;
            }
        }
        if (!window.DrawCall())
        {
            DrawNext(playerPos);
        }
    }
}
