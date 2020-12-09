using UnityEngine;

public static class Utility
{
    public static void SetCanvasGroupEnabled(CanvasGroup group, bool enabled)
    {
        group.alpha = enabled ? 1.0f : 0.0f;
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
        Debug.Log($"group={group.name} enabled={enabled}");
    }
}
