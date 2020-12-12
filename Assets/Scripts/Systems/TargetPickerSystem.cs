using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;

public class TargetPickerSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;

    public TargetPickerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ReadyToAttack);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isReadyToAttack = false;
            switch (entity.character.type)
            {
                case CharacterType.GoodGuy:
                    var target = CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.BadGuy)
                        .FirstOrDefault(e => e.isCurrentTarget);
                    AddTarget(entity, target);
                    break;
                case CharacterType.BadGuy:
                    var goodGuys = CharacterUtils.FindAll(_allCharacters.AsEnumerable(), CharacterType.GoodGuy);
                    target = CharacterUtils.FirstAliveCharacter(goodGuys);
                    AddTarget(entity, target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void AddTarget(GameEntity entity, GameEntity target)
    {
        if (target != null)
        {
            entity.AddTarget(target);
        }
    }
}