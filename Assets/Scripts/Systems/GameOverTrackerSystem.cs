using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class GameOverTrackerSystem : ReactiveSystem<GameEntity>
{
    private const string WinColor = "#97FF88";
    private const string LostColor = "#C3000A";

    private readonly IGroup<GameEntity> _allCharacters;

    public GameOverTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.GameState);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasGameState && entity.hasUI && entity.gameState.state == GameState.GameOver;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var entity = entities.First();
        if (!HasAliveCharacters(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.GoodGuy)))
        {
            PlayerLost(entity);
            return;
        }

        if (!HasAliveCharacters(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.BadGuy)))
        {
            PlayerWon(entity);
        }
    }

    private bool HasAliveCharacters(ICollection<GameEntity> characters)
    {
        return characters.Count > 0 && characters.Any(CharacterUtils.IsNotDead);
    }

    void PlayerWon(GameEntity entity)
    {
        entity.uI.gameResultText.color = transformColor(WinColor);
        entity.uI.gameResultText.text = "You Won";
        Utility.SetCanvasGroupEnabled(entity.uI.gameControlsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(entity.uI.endGameCanvasGroup, true);
    }

    void PlayerLost(GameEntity entity)
    {
        entity.uI.gameResultText.color = transformColor(LostColor);
        entity.uI.gameResultText.text = "You Lost";
        Utility.SetCanvasGroupEnabled(entity.uI.gameControlsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(entity.uI.endGameCanvasGroup, true);
    }

    private Color transformColor(string hexColor)
    {
        return ColorUtility.TryParseHtmlString(hexColor, out var newCol) ? newCol : Color.black;
    }
}