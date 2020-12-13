using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

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
        return new Collector<GameEntity>(
            new[]
            {
                context.GetGroup(GameMatcher.ActiveCharacter),
            }, new[]
            {
                GroupEvent.Added,
            }
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var target = GetTarget(entity);
            if (target != null)
            {
                Debug.Log($"target={target.view.gameObject.name}");
                entity.AddTarget(target);
            }
            else
            {
                entity.isActiveCharacter = false;
            }
        }
    }

    private GameEntity GetTarget(GameEntity entity)
    {
        GameEntity result;
        switch (entity.character.type)
        {
            case CharacterType.GoodGuy:
                var badGuys = CharacterUtils.FindBadGuys(_allCharacters.AsEnumerable());
                result = badGuys.FirstOrDefault(e => e.isCurrentTarget);
                break;

            case CharacterType.BadGuy:
                var goodGuys = CharacterUtils.FindGoodGuys(_allCharacters.AsEnumerable());
                result = CharacterUtils.FirstAliveCharacter(goodGuys);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return result;
    }
}