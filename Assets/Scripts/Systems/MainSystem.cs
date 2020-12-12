using System.Collections.Generic;
using System.Linq;
using Entitas;

public class MainSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;
    private readonly List<GameEntity> _goodGuys = new List<GameEntity>();
    private readonly List<GameEntity> _badGuys = new List<GameEntity>();
    private GameEntity _activeCharacter;

    public MainSystem(Contexts contexts)
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
        var gameStateEntity = entities.First();
        switch (gameStateEntity.gameState.state)
        {
            case GameState.NewRound:
                NewRound(gameStateEntity);
                break;
            case GameState.Play:
                Play(gameStateEntity);
                break;
            case GameState.AfterUserPressAttack:
                _activeCharacter.isReadyToAttack = true;
                gameStateEntity.ReplaceGameState(GameState.WaitingForAnimationEnd);

                break;
            case GameState.WaitingForAnimationEnd:
                if (!CharacterUtils.IsIdle(_activeCharacter))
                {
                    gameStateEntity.ReplaceGameState(GameState.Play);
                }
                break;
            case GameState.WaitingForUser:
            case GameState.GameOver:
                break;
        }
    }

    private void NewRound(GameEntity gameStateEntity)
    {
        _goodGuys.Clear();
        _goodGuys.AddRange(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.GoodGuy));
        _badGuys.Clear();
        _badGuys.AddRange(CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.BadGuy));
        gameStateEntity.ReplaceGameState(GameState.Play);
    }

    private void Play(GameEntity gameStateEntity)
    {
        foreach (var e in _goodGuys)
        {
            if (CharacterUtils.IsDead(e)) continue;
            if (CharacterUtils.FirstAliveCharacter(_badGuys) == null)
            {
                gameStateEntity.ReplaceGameState(GameState.GameOver);
                return;
            }

            _activeCharacter = e;

            // currentTarget.targetIndicator.gameObject.SetActive(true);
            // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, true);

            gameStateEntity.ReplaceGameState(GameState.WaitingForUser);

            // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, false);
            // currentTarget.targetIndicator.gameObject.SetActive(false);
        }

        foreach (var e in _badGuys)
        {
            if (CharacterUtils.IsDead(e)) continue;
            if (CharacterUtils.FirstAliveCharacter(_goodGuys) == null)
            {
                gameStateEntity.ReplaceGameState(GameState.GameOver);
                return;
            }

            _activeCharacter = e;
        }
    }
}