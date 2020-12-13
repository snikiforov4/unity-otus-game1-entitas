using System;
using System.Collections.Generic;
using Entitas;

public class CharacterMoveStartSystem : ReactiveSystem<GameEntity>
{
    public CharacterMoveStartSystem(Contexts contexts)
        : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Target);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasTarget;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        entities.ForEach(AttackEnemy);
    }

    private static void AttackEnemy(GameEntity entity)
    {
        switch (entity.character.weapon)
        {
            case Weapon.Bat:
            case Weapon.Fist:
                entity.ReplaceCharacterState(CharacterState.RunningToEnemy);
                break;

            case Weapon.Pistol:
                entity.ReplaceCharacterState(CharacterState.BeginShoot);
                break;

            default:
                throw new ArgumentOutOfRangeException($"unexpected weapon={entity.character.weapon}");
        }
    }
}