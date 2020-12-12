using System.Collections.Generic;
using Entitas;

public class HitTrackerSystem : ReactiveSystem<GameEntity>
{
    public HitTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CharacterHit);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasTarget;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isCharacterHit = false;
            e.target.value.AddDamage(1.0f);
        }
    }
}