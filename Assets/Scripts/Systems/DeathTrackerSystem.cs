using System.Collections.Generic;
using Entitas;

public class DeathTrackerSystem : ReactiveSystem<GameEntity>
{
    public DeathTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
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
            }
        }
    }
}