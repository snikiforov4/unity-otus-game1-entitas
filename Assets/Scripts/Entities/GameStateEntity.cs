using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateEntity : AbstractEntity
{
    public CanvasGroup gameControlsCanvasGroup;
    public CanvasGroup endGameCanvasGroup;
    public Button attackButton;
    public Button switchButton;
    public Button restartGameButton;
    public TextMeshProUGUI gameResultText;

    protected override void Start()
    {
        base.Start();
        entity.AddGameState(GameState.Initialize);
        entity.AddUI(gameControlsCanvasGroup, endGameCanvasGroup, gameResultText,
            attackButton, switchButton, restartGameButton);
        entity.isTargetSwitch = true;
        entity.isUserInput = false;
    }
}