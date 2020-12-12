using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;

public class AttackStarterSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;

    public AttackStarterSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
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
                entity.AddCharacterStateTransition(CharacterState.RunningToEnemy);
                break;

            case Weapon.Pistol:
                entity.AddCharacterStateTransition(CharacterState.BeginShoot);
                break;

            default:
                throw new ArgumentOutOfRangeException($"unexpected weapon={entity.character.weapon}");
        }
    }
}