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
        return entity.hasGameState && entity.gameState.state == GameState.GameOver;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (!HasAliveCharacters(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.GoodGuy)))
        {
            PlayerLost();
            return;
        }

        if (!HasAliveCharacters(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.BadGuy)))
        {
            PlayerWon();
        }
    }

    private bool HasAliveCharacters(ICollection<GameEntity> characters)
    {
        return characters.Count > 0 && characters.Any(CharacterUtils.IsNotDead);
    }

    void PlayerWon()
    {
        Debug.Log("Won");
        // SetGameResultTextColor(WinColor);
        // gameResultText.text = "You Won";
        // Utility.SetCanvasGroupEnabled(endGameCanvasGroup, true);
    }

    void PlayerLost()
    {
        Debug.Log("Lost");
        // SetGameResultTextColor(LostColor);
        // gameResultText.text = "You Lost";
        // Utility.SetCanvasGroupEnabled(endGameCanvasGroup, true);
    }

    private void SetGameResultTextColor(string hexColor)
    {
        // if (ColorUtility.TryParseHtmlString(hexColor, out Color newCol))
        // {
        // gameResultText.color = newCol;
        // }
    }
}