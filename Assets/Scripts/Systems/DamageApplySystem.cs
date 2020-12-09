using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DamageApplySystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public DamageApplySystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Damage);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasHealth && entity.hasDamage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var newHealthValue = Mathf.Max(e.health.value - e.damage.value, 0.0f);
            e.ReplaceHealth(newHealthValue);
            e.RemoveDamage();
        }
    }
}