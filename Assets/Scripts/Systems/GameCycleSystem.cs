using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;

public class GameCycleSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;
    private GameEntity _activeCharacter;

    public GameCycleSystem(Contexts contexts)
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
        return entity.hasGameState;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var entity = entities.First();
        switch (entity.gameState.state)
        {
            case GameState.Initialize:
                UpdateAllCharactersBeforeRoundStart();
                entity.ReplaceGameState(GameState.Play);
                break;

            case GameState.Play:
                Play(entity);
                break;

            case GameState.AfterUserPressAttack:
                _activeCharacter.isActiveCharacter = true;
                entity.ReplaceGameState(GameState.WaitForMoveEnd);
                break;

            case GameState.WaitForUser:
            case GameState.WaitForMoveEnd:
            case GameState.GameOver:
                break;
        }
    }

    private void UpdateAllCharactersBeforeRoundStart()
    {
        foreach (var entity in _allCharacters.AsEnumerable())
        {
            entity.isMoved = false;
        }
    }

    private void Play(GameEntity gameStateEntity)
    {
        var goodGuys = GetAlive(CharacterType.GoodGuy);
        var badGuys = GetAlive(CharacterType.BadGuy);
        if (IsGameOver(goodGuys, badGuys))
        {
            gameStateEntity.ReplaceGameState(GameState.GameOver);
            return;
        }

        foreach (var e in goodGuys)
        {
            if (e.isMoved) continue;

            // currentTarget.targetIndicator.gameObject.SetActive(true);

            _activeCharacter = e;
            gameStateEntity.isUserInput = true;
            return;

            // currentTarget.targetIndicator.gameObject.SetActive(false);
        }

        foreach (var e in badGuys)
        {
            if (e.isMoved) continue;
            e.isActiveCharacter = true;
            return;
        }

        gameStateEntity.ReplaceGameState(GameState.Initialize);
    }

    private bool IsGameOver(ICollection goodGuys, ICollection badGuys)
    {
        return goodGuys.Count == 0 || badGuys.Count == 0;
    }

    private List<GameEntity> GetAlive(CharacterType type)
    {
        return CharacterUtils.FindAll(_allCharacters.AsEnumerable(), type)
            .Where(CharacterUtils.IsNotDead)
            .ToList();
    }
}