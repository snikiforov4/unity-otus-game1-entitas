using System.Collections.Generic;
using Entitas;

public class UserInputHideSystem : ReactiveSystem<GameEntity>
{
    public UserInputHideSystem(Contexts contexts)
        : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return new Collector<GameEntity>(
            new[]
            {
                context.GetGroup(GameMatcher.UserInput),
            }, new[]
            {
                GroupEvent.AddedOrRemoved,
            }
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasUI;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var entity = entities.SingleEntity();
        Utility.SetCanvasGroupEnabled(entity.uI.gameControlsCanvasGroup, entity.isUserInput);
    }
}