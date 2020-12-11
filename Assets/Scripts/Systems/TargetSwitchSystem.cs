using System;
using System.Collections.Generic;
using System.Linq;
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
        return entity.isGameState && entity.isTargetSwitch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        entities.ForEach(e => e.isTargetSwitch = false);
        var enemies = FindAllBadGuys();

        var curTargetIdx = ArrayUtility.FindIndex(enemies, entity => entity.isCurrentTarget);
        Array.ForEach(enemies, entity => entity.isCurrentTarget = false);

        for (int i = 1; i < enemies.Length; i++)
        {
            var nextEnemy = enemies[(curTargetIdx + i) % enemies.Length];
            if (CharacterUtils.IsNotDead(nextEnemy))
            {
                nextEnemy.isCurrentTarget = true;
                return;
            }
        }
    }

    private GameEntity[] FindAllBadGuys()
    {
        return Array.FindAll(_allCharacters.GetEntities(),
            character => character.character.type == CharacterType.BadGuy);
    }
}