using System.Collections.Generic;
using Entitas;

public class CharacterHealthChangeSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public CharacterHealthChangeSystem(Contexts contexts)
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
        return entity.hasCharacter && entity.hasView && entity.hasHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        entities.ForEach(UpdateHealthIndicator);
    }

    private static void UpdateHealthIndicator(GameEntity e)
    {
        var healthIndicator = e.view.gameObject.GetComponentInChildren<HealthIndicator>();
        healthIndicator.UpdateHealth(e.health.value);
    }
}