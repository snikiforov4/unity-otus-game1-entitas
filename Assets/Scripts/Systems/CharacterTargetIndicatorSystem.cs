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
        return context.CreateCollector(new TriggerOnEvent<GameEntity>(GameMatcher.CurrentTarget,
            GroupEvent.AddedOrRemoved));
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
            targetIndicator.gameObject.SetActive(e.isCurrentTarget);
        }
    }
}