using System;
using System.Collections.Generic;
using Entitas;
using UnityEditor;

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
        var enemies = CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.BadGuy);
        
        var curTargetIdx = enemies.FindIndex(entity => entity.isCurrentTarget);
        enemies.ForEach(entity => entity.isCurrentTarget = false);

        for (int i = 1; i < enemies.Count; i++)
        {
            var nextEnemy = enemies[(curTargetIdx + i) % enemies.Count];
            if (CharacterUtils.IsNotDead(nextEnemy))
            {
                nextEnemy.isCurrentTarget = true;
                return;
            }
        }
    }
}