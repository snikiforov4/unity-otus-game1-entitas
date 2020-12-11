using System.Collections.Generic;
using Entitas;

public class CharacterNewStateApplySystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public CharacterNewStateApplySystem(Contexts contexts)
        : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CharacterStateTransition);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCharacter && entity.hasCharacterStateTransition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.character.state = entity.characterStateTransition.value;
            entity.RemoveCharacterStateTransition();
        }
    }
}