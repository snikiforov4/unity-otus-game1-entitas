using System.Collections.Generic;
using Entitas;

public class CharacterTargetIndicatorSystem : ReactiveSystem<GameEntity>
{
    public CharacterTargetIndicatorSystem(Contexts contexts)
        : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return new Collector<GameEntity>(
            new[]
            {
                context.GetGroup(GameMatcher.View),
                context.GetGroup(GameMatcher.CurrentTarget),
            }, new[]
            {
                GroupEvent.Added,
                GroupEvent.AddedOrRemoved,
            }
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasView;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var targetIndicator = e.view.gameObject.GetComponentInChildren<TargetIndicator>(true);
            targetIndicator.SetActive(e.isCurrentTarget);
        }
    }
}