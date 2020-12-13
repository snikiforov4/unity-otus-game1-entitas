using System.Collections.Generic;
using Entitas;

public class DeathTrackerSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _gameStateGroup;

    public DeathTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _gameStateGroup = contexts.game.GetGroup(GameMatcher.GameState);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (CharacterUtils.IsNotDead(entity) && entity.health.value <= 0)
            {
                entity.ReplaceCharacterState(CharacterState.BeginDying);
                if (entity.isCurrentTarget)
                {
                    _gameStateGroup.GetSingleEntity().isTargetSwitch = true;
                }
            }
        }
    }
}