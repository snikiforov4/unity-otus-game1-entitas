using System.Collections.Generic;
using Entitas;

public class CharacterTargetIndicatorHideSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;

    public CharacterTargetIndicatorHideSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
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
        var badGuys = CharacterUtils.FindBadGuys(_allCharacters.AsEnumerable());
        foreach (var e in badGuys)
        {
            var targetIndicator = e.view.gameObject.GetComponentInChildren<TargetIndicator>(true);
            targetIndicator.SetActive(entity.isUserInput && e.isCurrentTarget);
        }
    }
}