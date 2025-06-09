using System;
using UnityEngine.Events;

public static class CoreUISignals
{
    public static Action<UIPanelTypes, int> OnOpenPanel = delegate { };
    public static Action<int> OnClosePanel = delegate { };
    public static Action OnCloseAllPanels = delegate { };
}
