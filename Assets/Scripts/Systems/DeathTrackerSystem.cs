using System.Collections.Generic;
using Entitas;

public class DeathTrackerSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public DeathTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
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
            if (entity.character.state != CharacterState.Dead
                && entity.character.state != CharacterState.BeginDying
                && entity.health.value <= 0)
            {
                entity.AddCharacterStateTransition(CharacterState.BeginDying);
            }
        }
    }
}