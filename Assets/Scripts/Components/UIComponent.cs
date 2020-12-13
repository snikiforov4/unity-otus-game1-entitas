using Entitas;
using Entitas.CodeGeneration.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Unique]
public class UIComponent : IComponent
{
    public CanvasGroup gameControlsCanvasGroup;
    public CanvasGroup endGameCanvasGroup;
    public TextMeshProUGUI gameResultText;
    
    public Button attackButton;
    public Button switchButton;
    public Button restartGameButton;

}