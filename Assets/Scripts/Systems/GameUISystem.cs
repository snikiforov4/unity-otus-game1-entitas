using System.Collections.Generic;
using Entitas;
using UnityEngine.SceneManagement;

public class GameUISystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private GameEntity GameStateEntity => _contexts.game.GetGroup(GameMatcher.GameState).GetSingleEntity();

    public GameUISystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.UI);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasUI;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var ui = GameStateEntity.uI;
        ui.attackButton.onClick.AddListener(PlayerAttack);
        ui.switchButton.onClick.AddListener(NextTarget);
        ui.restartGameButton.onClick.AddListener(RestartGame);
        
        Utility.SetCanvasGroupEnabled(ui.gameControlsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(ui.endGameCanvasGroup, false);
    }

    private void PlayerAttack()
    {
        GameStateEntity.ReplaceGameState(GameState.AfterUserPressAttack);
        GameStateEntity.isUserInput = false;
    }

    private void NextTarget()
    {
        GameStateEntity.isTargetSwitch = true;
    }

    private static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}