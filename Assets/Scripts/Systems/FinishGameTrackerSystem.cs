using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class FinishGameTrackerSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    private readonly IGroup<GameEntity> _allCharacters;

    public FinishGameTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(new TriggerOnEvent<GameEntity>(
            GameMatcher.CharacterStateTransition, GroupEvent.AddedOrRemoved));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasCharacterStateTransition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (!HasAliveCharacters(FindAllCharacters(CharacterType.GoodGuy)))
        {
            PlayerLost();
            return;
        }

        if (!HasAliveCharacters(FindAllCharacters(CharacterType.BadGuy)))
        {
            PlayerWon();
        }
    }

    private ICollection<GameEntity> FindAllCharacters(CharacterType characterType)
    {
        return Array.FindAll(_allCharacters.GetEntities(), character => character.character.type == characterType);
    }

    private bool HasAliveCharacters(ICollection<GameEntity> characters)
    {
        return characters.Count > 0 && characters.Any(IsNotDead);
    }

    private static bool IsNotDead(GameEntity character)
    {
        var characterState = CharacterUtils.GetCharacterState(character);
        return characterState != CharacterState.Dead;
    }

    void PlayerWon()
    {
        Debug.Log("Lost");
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