using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public CanvasGroup gameControlsCanvasGroup;
    public CanvasGroup endGameCanvasGroup;
    public Button attackButton;
    public Button switchButton;
    public Button restartGameButton;
    public TextMeshProUGUI gameResultText;

    void Start()
    {
        attackButton.onClick.AddListener(PlayerAttack);
        switchButton.onClick.AddListener(NextTarget);
        restartGameButton.onClick.AddListener(RestartGame);
        Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, true);
        Utility.SetCanvasGroupEnabled(endGameCanvasGroup, false);
    }

    private static void PlayerAttack()
    {
        GetGameStateEntity().ReplaceGameState(GameState.AfterUserPressAttack);
    }

    private static void NextTarget()
    {
        GetGameStateEntity().isTargetSwitch = true;
    }

    private static GameEntity GetGameStateEntity()
    {
        return Contexts.sharedInstance.game.GetGroup(GameMatcher.GameState).GetSingleEntity();
    }

    private static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}