using System.Collections.Generic;
using Entitas;

public class CharacterMoveEndSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private GameEntity GameStateEntity => _contexts.game.GetGroup(GameMatcher.GameState).GetSingleEntity();

    public CharacterMoveEndSystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return new Collector<GameEntity>(
            new[]
            {
                context.GetGroup(GameMatcher.ActiveCharacter),
            }, new[]
            {
                GroupEvent.Removed,
            }
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isMoved = true;
            if (e.hasTarget) e.RemoveTarget();
        }
        GameStateEntity.ReplaceGameState(GameState.Play);
    }
}