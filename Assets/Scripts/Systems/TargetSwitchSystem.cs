using System.Collections.Generic;
using Entitas;

public class TargetSwitchSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;

    public TargetSwitchSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TargetSwitch);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasGameState && entity.isTargetSwitch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        entities.ForEach(e => e.isTargetSwitch = false);
        var badGuys = CharacterUtils.FindBadGuys(_allCharacters.AsEnumerable());
        
        var curTargetIdx = badGuys.FindIndex(entity => entity.isCurrentTarget);
        badGuys.ForEach(entity => entity.isCurrentTarget = false);

        for (int i = 1; i < badGuys.Count; i++)
        {
            var nextEnemy = badGuys[(curTargetIdx + i) % badGuys.Count];
            if (CharacterUtils.IsNotDead(nextEnemy))
            {
                nextEnemy.isCurrentTarget = true;
                return;
            }
        }
    }
}