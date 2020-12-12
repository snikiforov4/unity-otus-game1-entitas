using System.Collections.Generic;
using Entitas;

public class HitTrackerSystem : ReactiveSystem<GameEntity>
{
    private readonly IGroup<GameEntity> _allCharacters;

    public HitTrackerSystem(Contexts contexts)
        : base(contexts.game)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CharacterHit);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (TryFindEnemyFrom(GetOppositeCharacterType(e.character.type), out var enemy))
            {
                enemy.AddDamage(1.0f);
            }

            e.isCharacterHit = false;
        }
    }

    private bool TryFindEnemyFrom(CharacterType type, out GameEntity enemy)
    {
        enemy = null;
        foreach (var e in _allCharacters)
        {
            if (e.character.type == type && e.isCurrentTarget)
            {
                enemy = e;
                break;
            }
        }
        return enemy != null;
    }

    private CharacterType GetOppositeCharacterType(CharacterType type)
    {
        return type == CharacterType.BadGuy 
            ? CharacterType.GoodGuy 
            : CharacterType.BadGuy; 
    }
}